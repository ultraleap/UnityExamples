# A Sensation which creates a Polyline of 35 points of the finger joints, along which a Circle Path is animated.
from pysensationcore import *
import sensation_helpers as sh
import HandOperations

# We will use the joint positions of the fingers to animate a Circle along a PolylinePath
fingers = ["thumb", "indexFinger", "middleFinger", "ringFinger", "pinkyFinger"]
bones = ["metacarpal", "proximal", "intermediate", "distal", "intermediate","proximal","metacarpal"]

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
points = sh.createList(numPoints)

# Connect the points list for our Polylinepath to the animation path
connect(points["output"], animPath.points)

translateAlongPath = createInstance("TranslateAlongPath", "translateAlongPath")
connect(Constant((1,0,0)), translateAlongPath.direction)
connect(animPath.out, translateAlongPath.animationPath)

# The Object Path (a circle) Will trace along the animation Path
# On top of its translation along the path, we apply a rotation transform,
# to match the orientation of the Palm
circlePath = createInstance("CirclePath", "objectPath")

orientToPalmInstance = createInstance("OrientPathToPalm", "orientToPalm")

# Object Path -> OrientPathToPalm -> TranslateAlongPath
connect(circlePath.out, orientToPalmInstance.path)
connect(orientToPalmInstance.out, translateAlongPath.objectPath)

topLevelInputs = {}
for n in range(0,numPoints):
    topLevelInputs[(jointKeyFrames[n], points["inputs"][n])] = (0,0,0)

topLevelInputs[("t", translateAlongPath.t)] = (0, 0, 0)
topLevelInputs[("duration", translateAlongPath.duration)] = (2.5,0,0)
topLevelInputs[("dotSize", circlePath.radius)] = (0.01, 0, 0)
topLevelInputs[("palm_direction", orientToPalmInstance.palm_direction)] = (0, 0, 0)
topLevelInputs[("palm_normal", orientToPalmInstance.palm_normal)] = (0, 0, 0)

fingerScan = sh.createSensationFromPath("Finger Trace",
                                  topLevelInputs,
                                  output = translateAlongPath.out,
                                  drawFrequency = 120,
                                  renderMode=sh.RenderMode.Loop,
                                  definedInVirtualSpace = True
                                  )

# Hide the non-vital inputs...
visibleInputs = ("duration", "dotSize")
for topLevelInput in topLevelInputs.keys():
    inputName = topLevelInput[0]
    if inputName not in visibleInputs:
        setMetaData(getattr(fingerScan, inputName), "Input-Visibility", False)

setMetaData(fingerScan.duration, "Type", "Scalar")
setMetaData(fingerScan.dotSize, "Type", "Scalar")