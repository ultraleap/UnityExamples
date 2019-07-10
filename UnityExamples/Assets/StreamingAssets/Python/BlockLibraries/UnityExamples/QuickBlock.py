from pysensationcore import *

quickBlockCount = 0
def createQuickBlock(src, behavior, blockName=None):
    """
    Returns a block instance with a single output having the given 'behavior',
    a single input connected to the given 'src' and return the output port instance.
    This can be used to implement simple functions of one input inline during block
    definition
    """
    global quickBlockCount
    if not blockName:
        name = "QuickBlock%d" % quickBlockCount
    else:
        name = blockName

    quickBlockCount += 1
    b = defineBlock(name)
    defineInputs(b, "input")
    defineOutputs(b, "out")
    defineBlockOutputBehaviour(b.out, behavior)
    setMetaData(b.out, "Sensation-Producing", False)

    i = createInstance(name, name)
    connect(src, i.input)
    return i.out