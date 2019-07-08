from pysensationcore import *
import sensation_helpers as sh
import Generators

pulseWave = createInstance("PulseWaveRepeat", "pulse")
lissajousPath = createInstance("LissajousPath", "LissajousInstance")

lissajous = sh.createSensationFromPath("PulsingLissajous",
                                       {
                                           ("t", pulseWave.t) : (5, 0, 0),
                                           ("repetitions", pulseWave.repetitions) : (5, 0, 0),
                                           ("pulseLength", pulseWave.pulseLength) : (0.2, 0, 0),
                                           ("repeatGap", pulseWave.repeatGap) : (0.1, 0, 0),
                                           ("min", pulseWave.min) : (0, 0, 0),
                                           ("max", pulseWave.max) : (1, 0, 0),
                                           ("sizeX", lissajousPath.sizeX) : (0.01, 0.0, 0.0),
                                           ("sizeY", lissajousPath.sizeY) : (0.01, 0.0, 0.0),
                                           ("paramA", lissajousPath.paramA) : (3.0, 0.0, 0.0),
                                           ("paramB", lissajousPath.paramB) : (2.0, 0.0, 0.0)
                                       },
                                       output = lissajousPath.out,
                                       drawFrequency = 40,
                                       renderMode=sh.RenderMode.Loop,
                                       intensity = pulseWave.out
                                       )

setMetaData(lissajous, "Allow-Transform", True)
setMetaData(lissajous.repetitions, "Type", "Scalar")
setMetaData(lissajous.pulseLength, "Type", "Scalar")
setMetaData(lissajous.repeatGap, "Type", "Scalar")
setMetaData(lissajous.min, "Type", "Scalar")
setMetaData(lissajous.max, "Type", "Scalar")
setMetaData(lissajous.min, "Input-Visibility", False)
setMetaData(lissajous.max, "Input-Visibility", False)
setMetaData(lissajous.sizeX, "Type", "Scalar")
setMetaData(lissajous.sizeY, "Type", "Scalar")
setMetaData(lissajous.paramA, "Type", "Scalar")
setMetaData(lissajous.paramB, "Type", "Scalar")
