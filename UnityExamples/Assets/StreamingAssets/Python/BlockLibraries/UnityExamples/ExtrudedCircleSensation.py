from pysensationcore import *
import sensation_helpers as sh

block = defineBlock("ExtrudedCircle")
defineInputs(block,
             "objectCentre",	
             "radius",
             "extrusionDirection",
             "palm_position",
             "palm_normal")
defineOutputs(block, "out")

defineBlockInputDefaultValue(block.objectCentre, (0.0,0.2,0))
defineBlockInputDefaultValue(block.radius, (0.03,0,0))
defineBlockInputDefaultValue(block.palm_position, (0,0.2,0))
defineBlockInputDefaultValue(block.palm_normal, (0,0,0))
defineBlockInputDefaultValue(block.extrusionDirection, (0.0, 1.0, 0.0))

setMetaData(block.objectCentre, "Type", "Point")
setMetaData(block.radius, "Type", "Scalar")
setMetaData(block.extrusionDirection, "Input-Visibility", False)
setMetaData(block.palm_normal, "Input-Visibility", False)
setMetaData(block.palm_position, "Input-Visibility", False)

circle = createInstance("CirclePath", "circlePath")
connect(block.radius, circle.radius)

# Transform from Virtual Space to Emitter space
transformInstance = createInstance("ComposeTransform", "transformInstance")
connect(Constant((1, 0, 0)), transformInstance.x)
connect(Constant((0, 0, 1)), transformInstance.y)
connect(Constant((0, 1, 0)), transformInstance.z)
connect(block.objectCentre, transformInstance.o)

transformPathInstance = createInstance("TransformPath", "transformPathInstance")
connect(transformInstance.out, transformPathInstance.transform)
connect(circle.out, transformPathInstance.path)

projectedPath = createInstance("ProjectPathOntoPlane", "projectedPath")
connect(transformPathInstance.out, projectedPath.path)
connect(block.extrusionDirection, projectedPath.projectionDirection)
connect(block.palm_position, projectedPath.planePoint)
connect(block.palm_normal, projectedPath.planeNormal)

focalPoints = sh.createVirtualToPhysicalFocalPointPipeline(block,
                                                           projectedPath.out,
                                                           renderMode=sh.RenderMode.Loop,
                                                           drawFrequency = 70)

evalOnlyIfIntersecting = createInstance("Comparator", "evalOnlyIfIntersecting")
connect(projectedPath.valid, evalOnlyIfIntersecting.a)
connect(Constant((1,0,0)), evalOnlyIfIntersecting.b)
connect(Constant((0,0,0,0)), evalOnlyIfIntersecting.returnValueIfAGreaterThanB)
connect(focalPoints, evalOnlyIfIntersecting.returnValueIfAEqualsB)
connect(Constant((0,0,0,0)), evalOnlyIfIntersecting.returnValueIfALessThanB)
connect(evalOnlyIfIntersecting.out, block.out)
