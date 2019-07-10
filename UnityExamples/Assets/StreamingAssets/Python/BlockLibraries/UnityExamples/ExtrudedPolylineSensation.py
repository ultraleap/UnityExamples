from pysensationcore import *
import sensation_helpers as sh

block = defineBlock("ExtrudedPolyline")
defineInputs(block,
             "objectCentre",
             "point0",
             "point1",
             "point2",
             "point3",
             "point4",
             "extrusionDirection",
             "palm_position",
             "palm_normal")

defineOutputs(block, "out")
defineBlockInputDefaultValue(block.objectCentre, (0, 0.2, 0.0))
defineBlockInputDefaultValue(block.palm_position, (0,0,0))
defineBlockInputDefaultValue(block.palm_normal, (0,0,0))
defineBlockInputDefaultValue(block.point0, (0.025, 0.0, 0.025))
defineBlockInputDefaultValue(block.point1, (0.025, 0.0, -0.025))
defineBlockInputDefaultValue(block.point2, (-0.025,0.0, -0.025))
defineBlockInputDefaultValue(block.point3, (-0.025, 0.0, 0.025))
defineBlockInputDefaultValue(block.point4, (0.025, 0.0, 0.025))

setMetaData(block.objectCentre, "Type", "Point")
setMetaData(block.point0, "Input-Visibility", False)
setMetaData(block.point1, "Input-Visibility", False)
setMetaData(block.point2, "Input-Visibility", False)
setMetaData(block.point3, "Input-Visibility", False)
setMetaData(block.point4, "Input-Visibility", False)

setMetaData(block.palm_normal, "Input-Visibility", False)
setMetaData(block.palm_position, "Input-Visibility", False)

defineBlockInputDefaultValue(block.extrusionDirection, (0.0, 1.0, 0.0))

polyline = createInstance("PolylinePath", "polyline")
points = sh.createList(5)

connect(block.point0, points["inputs"][0])
connect(block.point1, points["inputs"][1])
connect(block.point2, points["inputs"][2])
connect(block.point3, points["inputs"][3])
connect(block.point4, points["inputs"][4])

connect(points["output"], polyline.points)

# Transform from Virtual Space to Emitter space
transformInstance = createInstance("ComposeTransform", "transformInstance")
connect(Constant((1, 0, 0)), transformInstance.x)
connect(Constant((0, 1, 0)), transformInstance.y)
connect(Constant((0, 0, 1)), transformInstance.z)
connect(block.objectCentre, transformInstance.o)

transformPathInstance = createInstance("TransformPath", "transformPathInstance")
connect(transformInstance.out, transformPathInstance.transform)
connect(polyline.out, transformPathInstance.path)

projectedPath = createInstance("ProjectPathOntoPlane", "projectedPath")
connect(transformPathInstance.out, projectedPath.path)
connect(block.extrusionDirection, projectedPath.projectionDirection)
connect(block.palm_position, projectedPath.planePoint)
connect(block.palm_normal, projectedPath.planeNormal)

focalPoints = sh.createVirtualToPhysicalFocalPointPipeline(block,
                                                           projectedPath.out,
                                                           renderMode=sh.RenderMode.Loop,
                                                           drawFrequency = 50)

evalOnlyIfIntersecting = createInstance("Comparator", "evalOnlyIfIntersecting")
connect(projectedPath.valid, evalOnlyIfIntersecting.a)
connect(Constant((1,0,0)), evalOnlyIfIntersecting.b)
connect(Constant((0,0,0,0)), evalOnlyIfIntersecting.returnValueIfAGreaterThanB)
connect(focalPoints, evalOnlyIfIntersecting.returnValueIfAEqualsB)
connect(Constant((0,0,0,0)), evalOnlyIfIntersecting.returnValueIfALessThanB)
connect(evalOnlyIfIntersecting.out, block.out)
