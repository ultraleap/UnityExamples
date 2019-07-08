from pysensationcore import *

quadToQuadIntersection = defineBlock("QuadToQuadIntersection")

defineInputs(quadToQuadIntersection, "up0", "right0", "center0", "up1", "right1", "center1")

quadInstance0 = createInstance("QuadFromVectors", "QuadFromVectorsInst0")
quadInstance1 = createInstance("QuadFromVectors", "QuadFromVectorsInst1")

connect(quadToQuadIntersection.up0, quadInstance0.up)
connect(quadToQuadIntersection.right0, quadInstance0.right)
connect(quadToQuadIntersection.center0, quadInstance0.center)

connect(quadToQuadIntersection.up1, quadInstance1.up)
connect(quadToQuadIntersection.right1, quadInstance1.right)
connect(quadToQuadIntersection.center1, quadInstance1.center)

crossProductInst0 = createInstance("CrossProduct", "CrossProductInst0")
crossProductInst1 = createInstance("CrossProduct", "CrossProductInst1")

connect(quadToQuadIntersection.up0, crossProductInst0.lhs)
connect(quadToQuadIntersection.right0, crossProductInst0.rhs)
connect(quadToQuadIntersection.up1, crossProductInst1.lhs)
connect(quadToQuadIntersection.right1, crossProductInst1.rhs)

planeToPlaneIntersectionInst = createInstance("PlaneToPlaneIntersection", "PlaneToPlaneIntersectionInst")
connect(quadToQuadIntersection.center0, planeToPlaneIntersectionInst.point0)
connect(crossProductInst0.out, planeToPlaneIntersectionInst.normal0)
connect(quadToQuadIntersection.center1, planeToPlaneIntersectionInst.point1)
connect(crossProductInst1.out, planeToPlaneIntersectionInst.normal1)

lineToQuadIntersectionInst0 = createInstance("LineToQuadIntersection", "LineToQuadIntersectionInst0")
lineToQuadIntersectionInst1 = createInstance("LineToQuadIntersection", "LineToQuadIntersectionInst1")

connect(quadInstance0.out, lineToQuadIntersectionInst0.quad)
connect(planeToPlaneIntersectionInst.out, lineToQuadIntersectionInst0.line)

connect(quadInstance1.out, lineToQuadIntersectionInst1.quad)
connect(planeToPlaneIntersectionInst.out, lineToQuadIntersectionInst1.line)

segmentsIntersectionsInst = createInstance("ColinearSegmentToSegmentIntersection", "segmentsIntersection")
connect(lineToQuadIntersectionInst0.out, segmentsIntersectionsInst.segment0)
connect(lineToQuadIntersectionInst1.out, segmentsIntersectionsInst.segment1)

toVector3Inst = createInstance("SegmentToVector3", "toVector3")
connect(segmentsIntersectionsInst.out, toVector3Inst.segment)

defineOutputs(quadToQuadIntersection, "endpointA", "endpointB")
connect(toVector3Inst.endpointA, quadToQuadIntersection.endpointA)
connect(toVector3Inst.endpointB, quadToQuadIntersection.endpointB)

setMetaData(quadToQuadIntersection.endpointA, "Sensation-Producing", False)
setMetaData(quadToQuadIntersection.endpointB, "Sensation-Producing", False)

