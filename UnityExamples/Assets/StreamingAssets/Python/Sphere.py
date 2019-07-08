from pysensationcore import *
import sensation_helpers as sh

import NearestPointOnPlane
import NonParallelVector
import RightTriangleSideLength

def connectWithTrace(src, dst):
    t = createInstance("Trace", "trace")
    connect(src, t.input)
    connect(t.out, dst)

def defineCircleTransformBlock():
    # A block to calculate the transform matrix needed to place a circle at the
    # plane-sphere intersection
    circleTransformBlock = defineBlock("SphereCircleTransform")
    defineInputs(circleTransformBlock,
                 "planeNormal",
                 "circleCentre")
    defineOutputs(circleTransformBlock, "out")
    setMetaData(circleTransformBlock.out, "Sensation-Producing", False)

    vectorNotParallelToPlaneNormal = createInstance("NonParallelVector", "vectorNotParallelToPlaneNormal")
    connect(circleTransformBlock.planeNormal, vectorNotParallelToPlaneNormal.v)

    firstOrthonormalVectorInPlane = createInstance("CrossProduct", "firstOrthonormalVectorInPlane")
    connect(circleTransformBlock.planeNormal, firstOrthonormalVectorInPlane.lhs)
    connect(vectorNotParallelToPlaneNormal.out, firstOrthonormalVectorInPlane.rhs)

    secondOrthonormalVectorInPlane = createInstance("CrossProduct", "secondOrthonormalVectorInPlane")
    connect(circleTransformBlock.planeNormal, secondOrthonormalVectorInPlane.lhs)
    connect(firstOrthonormalVectorInPlane.out, secondOrthonormalVectorInPlane.rhs)

    transform = createInstance("ComposeTransform", "transform")
    connect(firstOrthonormalVectorInPlane.normalized, transform.x)
    connect(secondOrthonormalVectorInPlane.normalized, transform.y)
    connect(circleTransformBlock.planeNormal, transform.z)
    connect(circleTransformBlock.circleCentre, transform.o)

    connect(transform.out, circleTransformBlock.out)

    return circleTransformBlock


planeSphereIntersectionBlock = defineBlock("PlaneSphereIntersection")
defineInputs(planeSphereIntersectionBlock,
             "sphereCentre",
             "sphereRadius",
             "planeNormal",
             "planePoint")
defineOutputs(planeSphereIntersectionBlock, "intersected", "out")

setMetaData(planeSphereIntersectionBlock.intersected, "Sensation-Producing", False)

calcCircleCentre = createInstance("NearestPointOnPlane", "circleCentre")
connect(planeSphereIntersectionBlock.planeNormal, calcCircleCentre.planeNormal)
connect(planeSphereIntersectionBlock.planePoint, calcCircleCentre.planePoint)
connect(planeSphereIntersectionBlock.sphereCentre, calcCircleCentre.point)

calcIntersected = createInstance("Comparator", "calcIntersected")
connect(calcCircleCentre.distance, calcIntersected.a)
connect(planeSphereIntersectionBlock.sphereRadius, calcIntersected.b)
connect(Constant((0,0,0)), calcIntersected.returnValueIfAGreaterThanB)
connect(Constant((0,0,0)), calcIntersected.returnValueIfAEqualsB)
connect(Constant((1,0,0)), calcIntersected.returnValueIfALessThanB)
connect(calcIntersected.out, planeSphereIntersectionBlock.intersected)

calcRadius = createInstance("RightTriangleSideLength", "calcRadius")
connect(planeSphereIntersectionBlock.sphereRadius, calcRadius.hypotenuse)
connect(calcCircleCentre.distance, calcRadius.side)

defineCircleTransformBlock()
circleTransform = createInstance("SphereCircleTransform", "circleTransform")
connect(planeSphereIntersectionBlock.planeNormal, circleTransform.planeNormal)
connect(calcCircleCentre.nearestPointOnPlane, circleTransform.circleCentre)

circlePath = createInstance("CirclePath", "circlePath")
connect(calcRadius.out, circlePath.radius)

circleLocatedInVirtualSpace = createInstance("TransformPath", "circleLocatedInVirtualSpace")
connect(circlePath.out, circleLocatedInVirtualSpace.path)
connect(circleTransform.out, circleLocatedInVirtualSpace.transform)

connect(circleLocatedInVirtualSpace.out, planeSphereIntersectionBlock.out)

sphereBlock = defineBlock("Sphere")
defineInputs(sphereBlock,
             "centre",
             "radius",
             "palm_normal",
             "palm_position")
defineBlockInputDefaultValue(sphereBlock.centre, (0, 0.25, 0))
defineBlockInputDefaultValue(sphereBlock.radius, (0.06, 0, 0))
defineBlockInputDefaultValue(sphereBlock.palm_normal, (0, 0, 1))
defineBlockInputDefaultValue(sphereBlock.palm_position, (0, 0, 0.211))

setMetaData(sphereBlock.radius, "Type", "Scalar")
setMetaData(sphereBlock.centre, "Type", "Point")
setMetaData(sphereBlock.palm_normal, "Input-Visibility", False)
setMetaData(sphereBlock.palm_position, "Input-Visibility", False)

defineOutputs(sphereBlock, "out")

planeSphereIntersection = createInstance("PlaneSphereIntersection", "planeSphereIntersection")
connect(sphereBlock.centre, planeSphereIntersection.sphereCentre)
connect(sphereBlock.radius, planeSphereIntersection.sphereRadius)
connect(sphereBlock.palm_normal, planeSphereIntersection.planeNormal)
connect(sphereBlock.palm_position, planeSphereIntersection.planePoint)

focalPoints = sh.createVirtualToPhysicalFocalPointPipeline(sphereBlock,
                                                           planeSphereIntersection.out,
                                                           renderMode = sh.RenderMode.Loop,
                                                           drawFrequency = 70)

evalOnlyIfIntersecting = createInstance("Comparator", "evalOnlyIfIntersecting")
connect(planeSphereIntersection.intersected, evalOnlyIfIntersecting.a)
connect(Constant((1,0,0)), evalOnlyIfIntersecting.b)
connect(Constant((0,0,0,0)), evalOnlyIfIntersecting.returnValueIfAGreaterThanB)
connect(focalPoints, evalOnlyIfIntersecting.returnValueIfAEqualsB)
connect(Constant((0,0,0,0)), evalOnlyIfIntersecting.returnValueIfALessThanB)
connect(evalOnlyIfIntersecting.out, sphereBlock.out)
