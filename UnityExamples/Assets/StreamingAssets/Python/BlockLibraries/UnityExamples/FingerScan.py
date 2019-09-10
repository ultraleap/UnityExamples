# A Sensation which scans an object along the index finger.
from pysensationcore import *
import sensation_helpers as sh

# We will use the joint positions of the fingers to animate an object along a PolylinePath
fingers = ["indexFinger"]
bones = ["proximal", "intermediate", "distal", "intermediate","proximal"]

jointKeyFrames = []

# Create a Polyline Path for each Animation Step
animPath = createInstance("PolylinePath", "PolylinePathInstance")

# Create inputs for each of the Bone joints
for finger in fingers:
    for bone in bones:
        jointInputName = "%s_%s_position" % (finger, bone)
        jointKeyFrames+=[jointInputName]

# The number of Key frames
numPoints = len(jointKeyFrames)

prefix = "point"
points = sh.createList(numPoints)

# Connect the points output to the animation path points
connect(points["output"], animPath.points)

translateAlongPath = createInstance("TranslateAlongPath", "translateAlongPath")
connect(Constant((1,0,0)), translateAlongPath.direction)
connect(animPath.out, translateAlongPath.animationPath)

# The Object Path (a circle) Will trace along the animation Path
# On top of its translation along the path, we apply a rotation transform,
# to match the orientation of the Palm
objectPath = createInstance("LissajousPath", "objectPath")
connect(Constant((0.005,0,0)), objectPath.sizeY)
connect(Constant((3,0,0)), objectPath.paramA)
connect(Constant((2,0,0)), objectPath.paramB)

crossProductInst = createInstance("CrossProduct", "crossProduct")

# Compose a Transform based on the palm orientation to orient the objectPath
composeTransform = createInstance("ComposeTransform", "ComposeObjInVtlSpaceTform")
connect(crossProductInst.out, composeTransform.x)
connect(Constant((0,0,0)), composeTransform.o)

transformPath = createInstance("TransformPath", "rotatePath")

# Object Path -> TransformPath -> TranslateAlongPath
connect(objectPath.out, transformPath.path)
connect(composeTransform.out, transformPath.transform)
connect(transformPath.out, translateAlongPath.objectPath)

topLevelInputs = {}
for n in range(0,numPoints):
    topLevelInputs[(jointKeyFrames[n], points["inputs"][n])] = (0,0,0)

# We want to make this be a finite Sensation
comparator = createInstance("Comparator", "ComparatorInstance")

# Inner block connections
connect(Constant((0, 0, 0)), comparator.returnValueIfAGreaterThanB)
connect(Constant((1, 0, 0)), comparator.returnValueIfAEqualsB)
connect(Constant((1, 0, 0)), comparator.returnValueIfALessThanB)

topLevelInputs[("t", translateAlongPath.t)] = (0, 0, 0)
topLevelInputs[("t", comparator.a)] = (0, 0, 0)
topLevelInputs[("duration", translateAlongPath.duration)] = (0.7,0,0)
topLevelInputs[("duration", comparator.b)] = (0.7,0,0)
topLevelInputs[("width", objectPath.sizeX)] = (0.01, 0, 0)
topLevelInputs[("palm_direction", crossProductInst.lhs)] = (0, 0, 0)
topLevelInputs[("palm_normal", crossProductInst.rhs)] = (0, 0, 0)
topLevelInputs[("palm_direction", composeTransform.y)] = (0, 0, 0)
topLevelInputs[("palm_normal", composeTransform.z)] = (0, 0, 0)

fingerScan = sh.createSensationFromPath("Finger Scan",
                                  topLevelInputs,
                                  output = translateAlongPath.out,
                                  drawFrequency = 40,
                                  renderMode=sh.RenderMode.Loop,
                                  intensity = comparator.out,
                                  definedInVirtualSpace = True
                                  )

# Hide the non-vital inputs, set others as scalars...
setMetaData(fingerScan.duration, "Type", "Scalar")
setMetaData(fingerScan.width, "Type", "Scalar")

visibleInputs = ("duration", "width")
for topLevelInput in topLevelInputs.keys():
    inputName = topLevelInput[0]
    if inputName not in visibleInputs:
        setMetaData(getattr(fingerScan, inputName), "Input-Visibility", False)
        

# The Finger Scan is a single shot (Finite) Sensation
setMetaData(fingerScan, "IsFinite", True)