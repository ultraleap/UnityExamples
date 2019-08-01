from pysensationcore import *

import sensation_helpers as sh
import IntensityWave
import Inverse

innerPath = createInstance("CirclePath","innerPath")
outerPath = createInstance("CirclePath","outerPath")
anim = createInstance("TranslateAlongPath","anim")
cosineWaveInstance = createInstance("IntensityWave","IntensityWaveInstance")
inverseInstance = createInstance("Inverse","InverseInstance")

connect(outerPath.out,anim.animationPath)
connect(innerPath.out,anim.objectPath)
connect(inverseInstance.out, anim.duration)
connect(Constant((-1, 0, 0)), anim.direction)

dial = sh.createSensationFromPath("Dial",
                                  {
                                      ("t", anim.t):(0, 0, 0),
                                      ("dotSize", innerPath.radius) : (0.005, 0, 0),
                                      ("ringSize", outerPath.radius) : (0.025, 0, 0),
                                      ("rate", inverseInstance.value) : (1, 0, 0),
                                      ("t", cosineWaveInstance.t) : (0, 0, 0)
                                  },
                                  output = anim.out,
                                  intensity = cosineWaveInstance.out,
                                  drawFrequency = 70,
                                  renderMode=sh.RenderMode.Loop
                                  )

setMetaData(dial, "Allow-Transform", True)
setMetaData(dial.dotSize, "Type", "Scalar")
setMetaData(dial.ringSize, "Type", "Scalar")
setMetaData(dial.rate, "Type", "Scalar")

setMetaData(dial.rate, "Min-Value", "-2")
setMetaData(dial.rate, "Max-Value", "2")