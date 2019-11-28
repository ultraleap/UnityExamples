"""
Provides convenience functions to allow a more concise definition of
new blocks than the sensationcore module alone.  For example:

from pysensationcore import *

b = defineBlock("B")
defineInputs(b, "a", "b")
defineOutputs(b, "out")
defineBlockOutputBehaviour(b.out, lambda inputs: ...)

c = defineBlock("C")
defineInputs(c, "x", "y")
defineOutputs(c, "out")
i = createInstance("B", "i")
connect(c.x, i.a)
connect(c.y, i.b)
connect(i.out, c.out)
"""

import sensationcore as sc
import textwrap

class _InstPort:

    def __init__(self, inst, port):
        self.inst = inst
        self.port = port


def _findOutputByNameOrNone(block, name):
    try:
        return sc.findOutputByName(block, name)
    except:
        return None

def _findInputByNameOrNone(block, name):
    try:
        return sc.findInputByName(block, name)
    except:
        return None


class _InstanceWrapper:

    def __init__(self, block, inst):
        self.block_ = block
        self.inst_ = inst

    def __getattr__(self, attrname):
        return _InstPort(self, getattr(self.block_, attrname))


class Block:
    """
    Wrap a sensationcore Block handle (for example returned from sensationcore.findBlock)
    so that the ports of that block appear as attributes of the Block object, e.g.

    b = Block(handle)
    outputOfB = b.out
    """

    def __init__(self, blockHandle):
        self.blockHandle_ = blockHandle

    def __getattr__(self, attrname):
        outputOrNone = _findOutputByNameOrNone(self.blockHandle_, attrname)
        if outputOrNone is not None:
            return outputOrNone

        inputOrNone = _findInputByNameOrNone(self.blockHandle_, attrname)
        if inputOrNone is not None:
            return inputOrNone

        raise RuntimeError("No port '" + attrname + "' found on Block")


def defineBlock(name):
    """
    Define a new Block and return its handle wrapped in an instance of the Block class.
    Use 'defineBlock' to create a new Block class with a given name.
    (See help on 'createInstance' for information on instancing a Block class in the Block Graph)

    :param name: Name of the Block to create
    :return: Block instance wrapping the created Block handle
    """
    return Block(sc.defineBlock(name))


def setMetaData(handle, key, value):
    """
    Use to define sensation producing metadata on a block

    :param handle: Handle on which to set metadata
    :param key: Name of metadata to set
    :param value: Value to set the metadata to
    :return: None
    """
    if isinstance(handle, Block):
        handle = handle.blockHandle_
    if isinstance(handle, _InstanceWrapper):
        handle = handle.block_.blockHandle_
    sc.setMetaData(handle, key, value)

def defineInputs(block, *names):
    """
    Use to define input ports for your Block.

    :param block: Block on which to create input ports
    :param names: names of the one or more ports to create (takes one or more input name arguments as strings)
    :return: List of input handles
    """
    return [sc.defineBlockInput(block.blockHandle_, n) for n in names]

def defineOutputs(block, *names):
    """
    Use to define output ports for your Block.

    :param block: Block on which to create output ports
    :param names: names of the one or more ports to create (takes one or more output name arguments as strings)
    :return: None
    """
    for n in names:
        sc.defineBlockOutput(block.blockHandle_, n)

def createInstance(blockName, instName):
    """
    Use to create instances of a Block.
    To use a Block in the Block Graph, an instance of it must be created.
    Multiple instances of a Block can exist in the same graph. For example:

    # Define two Circle Block instances from the Block Class named 'Circle'
    circleInstance1 = createInstance("Circle", "circle1")
    circleInstance2 = createInstance("Circle", "circle2")

    :param blockName: the class name of the Block to be found and instanced
    :param instName: the name of the instance to be created
    :return: implementation-defined type with attributes named as the ports of the Block named 'blockName'
             suitable for use in this module's 'connect' function
    """
    blockToInstance = sc.findBlock(blockName)
    return _InstanceWrapper(Block(blockToInstance), sc.createBlockInstance(blockToInstance, instName))

_constantCounter = 1
def Constant(value):
    """
    Produce an object suitable for use as a source in the 'connect' function that
    evaluates to the given 'value'

    :param value: Constant value to provide to a connected target
    :return: Output instance port of an instance of a Block that produces the given constant when evaluated
    """
    global _constantCounter
    blockName = "Constant" + str(_constantCounter)

    constBlock = defineBlock(blockName)
    defineOutputs(constBlock, "out")
    defineBlockOutputBehaviour(constBlock.out, lambda: value)
    setMetaData(constBlock.out, "Sensation-Producing", False)

    inst = createInstance(blockName, "constant" + str(_constantCounter))

    _constantCounter += 1

    return inst.out

def connect(source, target):
    """
    The main connection function for connecting Block instance inputs and outputs for Block Graph connectivity.

    Connection Scenarios:
    ---------------------
    Connect the output port named 'out' of an animator Block instance, to the 'radius' input port of a circleInstance:

    connect(animatorInstance.out, circleInstance.radius)

    Connect the 'final' output of some internal Block instance to the output of a structural Block:

    connect(internalBlock.out, myStructuralBlock.out)

    Connect a top-level input named 'intensity' of a structural Block to an internal Block's input:

    connect(topLevelBlock.intensity, internalBlock.intensity)

    Note: If your input/output names contain spaces or special characters, use the getattr(BLOCKCLASS, 'some input name') method, e.g

    connect(getattr(myBlock,"Radius (m)"), circleInstance.radius)

    :param source: a Block attribute representing a Block input port or an instance-port attribute (on an object
                   returned by 'createInstance') representing an instance output
    :param target: a Block attribute representing a Block output port or an instance-port attribute (on an object
                   returned by 'createInstance') representing an instance input
    :return: None
    """

    if isinstance(source, _InstPort) and isinstance(target, _InstPort):
        sc.connectChildren(source.inst.inst_, source.port, target.inst.inst_, target.port)
    elif isinstance(source, _InstPort):
        instwrapper = source.inst
        outputPortHandle = source.port
        sc.connectChildOutputToParentOutput(instwrapper.inst_, outputPortHandle, target)
    elif isinstance(target, _InstPort):
        instwrapper = target.inst
        inputPortHandle = target.port
        sc.connectParentInputToChildInput(source, instwrapper.inst_, inputPortHandle)
    else:
        raise TypeError("Connection source and target must be ports on Blocks or instances")

def defineBlockOutputBehaviour(output, behaviour):
    """
    Attach to a given output port a python function that will be evaluated to
    determine the value at that port during block network evaluation.

    :param output: port to which a behaviour function is to be attached
    :param behaviour: function to attach as behaviour to the given output port
    :return: None
    """
    sc.defineBlockOutputBehaviour(output, behaviour)

def defineBlockInputDefaultValue(input, default):
    """
    Define the default value to which an input will evaluate if a value is not
    otherwise provided for it either as a top-level input or as an unconnected
    instance input.

    :param input: port on which to set a default value
    :param default: value the port will evaluate to if no other means to determine the value is used
    :return: None
    """
    sc.defineBlockInputDefaultValue(input, default)

def attachDocumentation(entity, doc):
    """
    Attach documentation to the entity referred to by the given handle

    :param entity: entity to which documentation will be attached (block or port)
    :param doc: string containing the documentation to attach
    :return: None
    """
    strippedDoc = textwrap.dedent(doc).lstrip().rstrip()
    if isinstance(entity, Block):
        sc.setMetaData(entity.blockHandle_, "Docs.Description", strippedDoc)
    else:
        sc.setMetaData(entity, "Docs.Description", strippedDoc)

