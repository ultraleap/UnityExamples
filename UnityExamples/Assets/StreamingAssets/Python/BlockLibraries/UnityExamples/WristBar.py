from pysensationcore import *
import sensation_helpers as sh
import Ops

# # We will project a PolylinePath on to the Plane defined by the palm, and allow it to be dragged up and down the palm 

bar = createInstance("PolylinePath", "bar")
prefix = "point"
points = sh.createList(5)

halfWidth = 0.05
halfDepth = 0.007
connect(Constant((-halfWidth, -halfDepth, 0)), points["inputs"][0])
connect(Constant((halfWidth, -halfDepth, 0)), points["inputs"][1])
connect(Constant((halfWidth, halfDepth, 0)), points["inputs"][2])
connect(Constant((-halfWidth, halfDepth, 0)), points["inputs"][3])
connect(Constant((-halfWidth, -halfDepth, 0)), points["inputs"][4])

connect(points["output"], bar.points)
path = bar.out

# Compose a Transform
composeTransform = createInstance("ComposeTransform", "composeTransform")
connect(Constant((1,0,0)), composeTransform.x)
connect(Constant((0,1,0)), composeTransform.y)
connect(Constant((0,0,1)), composeTransform.z)

# We also need a Vector3 to produce an offset up and down the hand
offsetVector = createInstance("ComposeVector3", "vec3")
connect(Constant((0,0,0)), offsetVector.x)
connect(Constant((0,0,0)), offsetVector.z)

connect(offsetVector.out, composeTransform.o)

# A Transformation to move the Bar up-down in Sensation Space
transformPath = createInstance("TransformPath", "transform")

connect(composeTransform.out, transformPath.transform)
connect(path, transformPath.path)

handBar = sh.createSensationFromPath("Wrist Bar",
						{
						("offset", offsetVector.y) : (-0.06, 0.0, 0.0)
						},
                        output = transformPath.out,
                        drawFrequency = 90,
                        definedInVirtualSpace = False,
                        renderMode = sh.RenderMode.Loop
                        )

setMetaData(handBar.offset, "Type", "Scalar")
setMetaData(handBar.offset, "Min-Value", "-0.1")
setMetaData(handBar.offset, "Max-Value", "0.1")

setMetaData(handBar, "Allow-Transform", True)