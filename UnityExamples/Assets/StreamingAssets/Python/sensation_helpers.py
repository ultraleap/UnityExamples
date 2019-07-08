import pysensationcore as psc

topLevelBlockInputs = []

class RenderMode:
    Bounce = 0
    Loop = 1

def _isInputDefined(block, inputName):
    try:
        getattr(block, inputName)
        return True
    except:
        return False

def _hideInputs(inputHandles):
    for inputHandle in inputHandles:
        psc.setMetaData(inputHandle, "Input-Visibility", False)

def _mapInnerBlocksInputsToTopLevelInputs(inputs, pathSensation):
    for innerBlockPort in inputs:
        topLevelPortName = innerBlockPort[0]
        innerBlockPortHandle = innerBlockPort[1]

        if not _isInputDefined(pathSensation, topLevelPortName):
            psc.defineInputs(pathSensation, topLevelPortName)

        psc.defineBlockInputDefaultValue(getattr(pathSensation, topLevelPortName), inputs[innerBlockPort])
        psc.connect(getattr(pathSensation, topLevelPortName), innerBlockPortHandle)

def _getRenderModeValue(renderMode):
    return psc.Constant((renderMode, 0, 0))

def addInputIfNotDefined(block, inputName, defaultValue=None, type=None, hidden=False, doc=None):
    if not _isInputDefined(block, inputName):
        psc.defineInputs(block, inputName)
        if defaultValue is not None:
            psc.defineBlockInputDefaultValue(getattr(block, inputName), defaultValue)
        if type is not None:
            psc.setMetaData(getattr(block, inputName), "Type", type)
        if hidden:
            psc.setMetaData(getattr(block, inputName), "Input-Visibility", False)
        if doc is not None:
            psc.setMetaData(getattr(block, inputName), "Docs.Description", doc)

        topLevelBlockInputs.append({"name" : inputName, "type" : type, "input-visibility" : not hidden, "default": defaultValue})

def transformPathSpace(pathSensation, path, fromSpaceToSpaceVectors, inverse=False,
                       composeTransformInstanceName="composeTransform",
                       transformPathInstanceName="transformPath"):

    (x,y,z,o) = fromSpaceToSpaceVectors

    if inverse:
        composeBlockToInstance = "ComposeInverseTransform"
    else:
        composeBlockToInstance = "ComposeTransform"

    composeTransform = \
        psc.createInstance(composeBlockToInstance, composeTransformInstanceName)
    transformPath = \
        psc.createInstance("TransformPath", transformPathInstanceName)

    psc.connect(x, composeTransform.x)
    psc.connect(y, composeTransform.y)
    psc.connect(z, composeTransform.z)
    psc.connect(o, composeTransform.o)

    psc.connect(composeTransform.out, transformPath.transform)
    psc.connect(path, transformPath.path)

    return transformPath.out

def createVirtualToPhysicalFocalPointPipeline(pathSensation, pathInVirtualSpace, drawFrequency, renderMode=RenderMode.Bounce, intensity=None):
    # 'view' the sensation from the vantage point of the virtual emitter in preparation
    # for converting to physical emitter space
    addInputIfNotDefined(pathSensation, "virtualEmitterXInVirtualSpace", (1, 0, 0), hidden=True, type="Vector3")
    addInputIfNotDefined(pathSensation, "virtualEmitterYInVirtualSpace", (0, 1, 0), hidden=True, type="Vector3")
    addInputIfNotDefined(pathSensation, "virtualEmitterZInVirtualSpace", (0, 0, 1), hidden=True, type="Vector3")
    addInputIfNotDefined(pathSensation, "virtualEmitterOriginInVirtualSpace", (0, 0, 0), hidden=True, type="Vector3")
    virtualEmitterInVirtualSpaceVectors = (pathSensation.virtualEmitterXInVirtualSpace,
                                           pathSensation.virtualEmitterYInVirtualSpace,
                                           pathSensation.virtualEmitterZInVirtualSpace,
                                           pathSensation.virtualEmitterOriginInVirtualSpace)
    pathInVirtualEmitterSpace = transformPathSpace(pathSensation, pathInVirtualSpace,
                                                   virtualEmitterInVirtualSpaceVectors,
                                                   inverse=True,
                                                   composeTransformInstanceName="ComposeInverseEmitterTransformInstance",
                                                   transformPathInstanceName="TransformPathVirtualToVirtualEmitter")
    # Transform from Virtual Emitter to Physical Emitter
    addInputIfNotDefined(pathSensation, "virtualXInEmitterSpace", (1, 0, 0), hidden=True, type="Vector3")
    addInputIfNotDefined(pathSensation, "virtualYInEmitterSpace", (0, 1, 0), hidden=True, type="Vector3")
    addInputIfNotDefined(pathSensation, "virtualZInEmitterSpace", (0, 0, 1), hidden=True, type="Vector3")
    addInputIfNotDefined(pathSensation, "virtualOriginInEmitterSpace", (0, 0, 0), hidden=True, type="Vector3")
    virtualEmitterToPhysicalEmitterVectors = (pathSensation.virtualXInEmitterSpace,
                                              pathSensation.virtualYInEmitterSpace,
                                              pathSensation.virtualZInEmitterSpace,
                                              pathSensation.virtualOriginInEmitterSpace)
    pathInPhysicalEmitterSpace = transformPathSpace(pathSensation, pathInVirtualEmitterSpace,
                                                    virtualEmitterToPhysicalEmitterVectors,
                                                    composeTransformInstanceName="ComposeVirtualToEmitterSpaceTransformInstance",
                                                    transformPathInstanceName="TransformPathVirtualToEmitterSpaceInstance")
    # Convert from path to focal point at time instant
    addInputIfNotDefined(pathSensation, "t", type="Scalar", doc="Time in seconds since the start of the sensation")
    addInputIfNotDefined(pathSensation, "drawFrequency", defaultValue=(drawFrequency, 0, 0), type="Scalar",
                         doc="Number of times per second the haptic path is rendered")
    renderPathInstance = psc.createInstance("RenderPath", "RenderPathInstance")
    psc.connect(pathSensation.t, renderPathInstance.t)
    psc.connect(pathSensation.drawFrequency, renderPathInstance.drawFrequency)
    psc.connect(pathInPhysicalEmitterSpace, renderPathInstance.path)
    psc.connect(_getRenderModeValue(renderMode), renderPathInstance.renderMode)

    # Incorporate intensity control
    setIntensityInstance = psc.createInstance("SetIntensity", "SetIntensityInstance")
    psc.connect(renderPathInstance.out, setIntensityInstance.point)
    if intensity is not None:
        psc.connect(intensity, setIntensityInstance.intensity)
    else:
        addInputIfNotDefined(pathSensation, "intensity", defaultValue=(1, 0, 0), type="Scalar",
                             doc="Control point intensity scale factor")
        psc.connect(pathSensation.intensity, setIntensityInstance.intensity)

    return setIntensityInstance.out


def createSensationFromPath(sensationName, inputs, output, drawFrequency=100, intensity=None, definedInVirtualSpace=False, renderMode=RenderMode.Bounce):
    """
    Defines a block to output a path as provided by the instance.

    It creates additional inputs to the block which relate to a transformation pipeline
    which transforms the path as follows:
    Sensation Space -> Virtual Space -> Object in Virtual Space -> Emitter Space

    :param sensationName: Name of the Block to create
    :param inputs: Dictionary of inputs of the path generating block: {("nameTopLevelInput", handleInnerBlockInput):  defaultValue}
    :param output: Instance output of the path generating block instance
    :param drawFrequency: Frequency in which to draw the path
    :param intensity: Instance output of the block instance that sets the intensity of the sensation
    :param definedInVirtualSpace: Boolean value (default=False). Set to True if the path is defined in Virtual Space (e.g. when using virtual hand tracking data), use False if path is defined in Sensation Space.
    :param renderMode: Enum value that specifies how the path will be rendered (RenderMode.Bounce, RenderMode.Loop)
    :return: A block named as specified with the transformation pipeline appended to the path block
    """

    pathSensation = psc.defineBlock(sensationName)

    _mapInnerBlocksInputsToTopLevelInputs(inputs, pathSensation)
    psc.defineOutputs(pathSensation,"out")

    # Transform from Sensation Space to virtual space unless the given path is already
    # defined to be in virtual space
    if definedInVirtualSpace:
        pathInVirtualSpace = output
    else:
        addInputIfNotDefined(pathSensation, "sensationXInVirtualSpace", (1, 0, 0), hidden=True, type="Vector3")
        addInputIfNotDefined(pathSensation, "sensationYInVirtualSpace", (0, 1, 0), hidden=True, type="Vector3")
        addInputIfNotDefined(pathSensation, "sensationZInVirtualSpace", (0, 0, 1), hidden=True, type="Vector3")
        addInputIfNotDefined(pathSensation, "sensationOriginInVirtualSpace", (0, 0, 0), hidden=True, type="Vector3")
        sensationInVirtualSpaceVectors = (pathSensation.sensationXInVirtualSpace,
                                          pathSensation.sensationYInVirtualSpace,
                                          pathSensation.sensationZInVirtualSpace,
                                          pathSensation.sensationOriginInVirtualSpace)
        pathInVirtualSpacePriorToTracking = transformPathSpace(pathSensation, output, sensationInVirtualSpaceVectors,
                                                               composeTransformInstanceName="ComposeSensationToVirtualSpaceTransformInstance",
                                                               transformPathInstanceName="TransformPathSensationToVirtualSpaceInstance")

        addInputIfNotDefined(pathSensation, "virtualObjectXInVirtualSpace", (1, 0, 0), hidden=True, type="Vector3")
        addInputIfNotDefined(pathSensation, "virtualObjectYInVirtualSpace", (0, 1, 0), hidden=True, type="Vector3")
        addInputIfNotDefined(pathSensation, "virtualObjectZInVirtualSpace", (0, 0, 1), hidden=True, type="Vector3")
        addInputIfNotDefined(pathSensation, "virtualObjectOriginInVirtualSpace", (0, 0, 0), hidden=True, type="Vector3")

        trackedObjectInVirtualSpaceVectors = (pathSensation.virtualObjectXInVirtualSpace,
                                              pathSensation.virtualObjectYInVirtualSpace,
                                              pathSensation.virtualObjectZInVirtualSpace,
                                              pathSensation.virtualObjectOriginInVirtualSpace)

        # Transform the path to the desired location in virtual space
        pathInVirtualSpace = transformPathSpace(pathSensation, pathInVirtualSpacePriorToTracking, trackedObjectInVirtualSpaceVectors,
                                                composeTransformInstanceName="ComposeObjectInVirtualSpaceTransformInstance",
                                                transformPathInstanceName="TransformPathToObjectInVirtualSpaceInstance")

    focalPoints = createVirtualToPhysicalFocalPointPipeline(pathSensation,
                                                            pathInVirtualSpace,
                                                            drawFrequency,
                                                            renderMode,
                                                            intensity)
    psc.connect(focalPoints, pathSensation.out)

    return pathSensation

def expandListToIndividualInputs(listInstanceInput, namePrefix, listSize):
    """
    Creates an instance network that constructs a list and connects its output to
    an input instance, which should expect a list.

    :param listInstanceInput: The input instance to which the constructed list should be connected
    :param namePrefix: The prefix that should be used when naming the instances created by this helper
    :param listSize: The length of the list to be constructed
    :return: A list of the input instances to the list-constructing network
    """
    emptyListInst = psc.createInstance("EmptyList", namePrefix + "EmptyListInstance")
    inputInstances = [None] * listSize

    previousOutput = emptyListInst.out
    for i in range(listSize):
        inputName = namePrefix + str(i)
        listAppendInst = psc.createInstance("ListAppend", inputName + "ListAppendInstance")
        psc.connect(previousOutput, listAppendInst.list)
        inputInstances[i] = listAppendInst.element
        previousOutput = listAppendInst.out

    psc.connect(previousOutput, listInstanceInput)
    return inputInstances

def createList(listSize):
    """
    Creates list block that creates input instances for each element and an output instance for connecting to
    the resulting list. List size is limited to 300 elements. Larger lists will be truncated.

    :param listSize: The size of the list of point inputs that will be created
    :return: A list of the input instances and the output of the list block
    """
    listInst = psc.createInstance("ListBlock", "ListInstance")
    inputInstances = [None] * listSize

    psc.connect(psc.Constant((listSize, 0, 0)), listInst.size)

    for i in range(listSize):
        inputInstances[i] = getattr(listInst, "element" + str(i))

    return {"inputs":inputInstances, "output":listInst.out}
