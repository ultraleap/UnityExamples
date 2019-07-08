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
connect(Constant((1, 0, 0)), anim.direction)

palmTrackedDial = sh.createSensationFromPath("PalmTrackedDial",
                                             {
                                                 ("t", anim.t) : (0, 0, 0),
                                                 ("innerRadius", innerPath.radius) : (0.005, 0, 0),
                                                 ("outerRadius", outerPath.radius) : (0.025, 0, 0),
                                                 ("rate", inverseInstance.value) : (-1, 0, 0),
                                                 ("t", cosineWaveInstance.t) : (0, 0, 0)
                                             },
                                             output = anim.out,
                                             intensity = cosineWaveInstance.out,
                                             drawFrequency = 70,
                                             renderMode = sh.RenderMode.Loop
                                             )

setMetaData(palmTrackedDial.innerRadius, "Type", "Scalar")
setMetaData(palmTrackedDial.outerRadius, "Type", "Scalar")
setMetaData(palmTrackedDial.rate, "Type", "Scalar")
