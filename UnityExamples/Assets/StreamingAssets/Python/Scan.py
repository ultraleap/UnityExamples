from pysensationcore import *

import PointPair

scanBlock = defineBlock("Scan")
defineInputs(scanBlock,
             "t",
             "duration",
             "barLength",
             "barDirection",
             "animationPathStart",
             "animationPathEnd")
defineBlockInputDefaultValue(scanBlock.duration, (2.5, 0, 0))
defineBlockInputDefaultValue(scanBlock.barLength, (0.1, 0, 0))
defineBlockInputDefaultValue(scanBlock.barDirection, (1, 0, 0))
defineBlockInputDefaultValue(scanBlock.animationPathStart, (0, -0.06, 0.0))
defineBlockInputDefaultValue(scanBlock.animationPathEnd, (0, 0.06, 0.0))

animPath = createInstance("LinePath", "animPath")
connect(scanBlock.animationPathStart, animPath.endpointA)
connect(scanBlock.animationPathEnd, animPath.endpointB)

barEnds = createInstance("PointPair", "barEnds")
connect(scanBlock.barDirection, barEnds.direction)
connect(scanBlock.barLength, barEnds.distance)

bar = createInstance("LinePath", "bar")
connect(barEnds.negative, bar.endpointA)
connect(barEnds.positive, bar.endpointB)

anim = createInstance("TranslateAlongPath", "anim")
connect(scanBlock.t, anim.t)
connect(scanBlock.duration, anim.duration)
connect(Constant((0,0,0)), anim.direction)
connect(animPath.out, anim.animationPath)
connect(bar.out, anim.objectPath)

defineOutputs(scanBlock, "out")
setMetaData(scanBlock.out, "Sensation-Producing", False)
connect(anim.out, scanBlock.out)
