# Basic Rose Curve Sensation, fixed above array at 20cm
from pysensationcore import *
from math import sin, cos, pi, gcd

def angle_range(numerator, denominator):
    """
    Returns the range of angle required to complete a full rose curve path for given
    numerator and denominator
    """
    isWholeNumber = abs((float(numerator) / float(denominator)) % 1) == 0
    if isWholeNumber:
        k = numerator / denominator;
        pi_loops = 2*pi if ( k % 2 == 0) else pi
        return pi_loops
    else:
        GCD = gcd(int(numerator), int(denominator))
        simplifiedNumerator = numerator / GCD
        simplifiedDenominator = denominator / GCD;
        NDisOddOrEvenFactor = 2.0 if ((simplifiedNumerator*simplifiedDenominator) % 2 == 0) else 1.0
        pi_loops = (pi * NDisOddOrEvenFactor * simplifiedDenominator)
        return pi_loops

# === Rose Block ===
# A Rose Curve Sensation-producing Block
roseBlock = defineBlock("Rose")
defineInputs(roseBlock, "t", "radius", "numerator", "denominator", "drawFrequency")
defineBlockInputDefaultValue(roseBlock.radius, (0.02, 0.0, 0.0))
defineBlockInputDefaultValue(roseBlock.numerator, (7, 0.0, 0.0))
defineBlockInputDefaultValue(roseBlock.denominator, (3, 0.0, 0.0))
defineBlockInputDefaultValue(roseBlock.drawFrequency, (40, 0.0, 0.0))

def roseCurve(inputs):
    t = inputs[0][0]
    radius = inputs[1][0]
    numerator = inputs[2][0]
    denominator = inputs[3][0]
    
    f = inputs[4][0]
    
    # Avoid division by zero when entering values
    if denominator == 0:
        return (0,0,0)
    
    k = float(numerator)/float(denominator)
    
    theta = angle_range(numerator, denominator)
    
    x = (cos(t*theta*k*f) * cos(theta*t*f) * radius)
    y = (cos(t*theta*k*f) * sin(theta*t*f) * radius)
    
    return (x, y, 0.2)

defineOutputs(roseBlock, "out")
defineBlockOutputBehaviour(roseBlock.out, roseCurve)
setMetaData(roseBlock.out, "Sensation-Producing", True)
setMetaData(roseBlock.radius, "Type", "Scalar")
setMetaData(roseBlock.numerator, "Type", "Scalar")
setMetaData(roseBlock.denominator, "Type", "Scalar")
setMetaData(roseBlock.drawFrequency, "Type", "Scalar")
