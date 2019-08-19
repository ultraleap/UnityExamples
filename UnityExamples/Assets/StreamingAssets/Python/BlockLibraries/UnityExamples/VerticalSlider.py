from pysensationcore import *
import sensation_helpers as sh
import Ops

# A 'VerticalSlider' sensation which indicates the 'value' or amount of an 
# imagined 'vertical slider', running up the back of the hand/wrist (value=0)
# towards the top of the hand/middle fingertip (value=1)

bar = createInstance("PolylinePath", "bar")
prefix = "point"
points = sh.createList(5)

halfWidth = 0.007
halfDepth = 0.07

point0 = createInstance("ComposeVector3", "vec30")
connect(Constant((-halfWidth,0,0)), point0.x)
connect(Constant((0,0,0)), point0.z)

point1 = createInstance("ComposeVector3", "vec31")
connect(Constant((halfWidth,0,0)), point1.x)
connect(Constant((0,0,0)), point1.z)

# These two points (2+3) will change their y-values based on the value slider
point2 = createInstance("ComposeVector3", "vec32")
connect(Constant((halfWidth,0,0)), point2.x)
connect(Constant((0,0,0)), point2.z)

point3 = createInstance("ComposeVector3", "vec33")
connect(Constant((-halfWidth,0,0)), point3.x)
connect(Constant((0,0,0)), point3.z)

# This point loops back to the first
point4 = createInstance("ComposeVector3", "vec34")
connect(Constant((-halfWidth,0,0)), point4.x)
connect(Constant((0,0,0)), point4.z)
connect(Constant((halfWidth, -halfDepth, 0)), points["inputs"][1])


connect(point0.out, points["inputs"][0])
connect(point1.out, points["inputs"][1])
connect(point2.out, points["inputs"][2])
connect(point3.out, points["inputs"][3])
connect(point4.out, points["inputs"][4])

connect(points["output"], bar.points)
path = bar.out

# We need a linear interpolator, from y0 (value=0) to y1 (value=1)
# to drive the y-values of point 2 and 3, such that the length of the
# bar is dynamic.
lerp = createInstance("Lerp", "interpolator")
connect(Constant((0,0,0)), lerp.y0)
connect(Constant((1,0,0)), lerp.y1)

connect(lerp.out, point2.y)
connect(lerp.out, point3.y)

handBar = sh.createSensationFromPath("VerticalSlider",
						{
						("value", lerp.x) : (1, 0.0, 0.0),
						("yMin", lerp.x0) : (-halfDepth,0,0),
						("yMax", lerp.x1) : (halfDepth,0,0),
						("yMin", point0.y) : (-halfDepth,0,0),
						("yMin", point1.y) : (-halfDepth,0,0),
						("yMin", point4.y) : (-halfDepth,0,0)
						},
                        output = path,
                        drawFrequency = 90,
                        definedInVirtualSpace = False,
                        renderMode = sh.RenderMode.Loop
                        )

setMetaData(handBar.yMin, "Type", "Scalar")
setMetaData(handBar.yMin, "Min-Value", "-0.1")
setMetaData(handBar.yMin, "Max-Value", "0.1")
setMetaData(handBar.yMax, "Type", "Scalar")
setMetaData(handBar.yMax, "Min-Value", "-0.1")
setMetaData(handBar.yMax, "Max-Value", "0.1")

setMetaData(handBar.value, "Type", "Scalar")
setMetaData(handBar.value, "Min-Value", "0.01")
setMetaData(handBar.value, "Max-Value", "1")
setMetaData(handBar.value, "Max-Value", "1")
setMetaData(handBar, "Allow-Transform", True)