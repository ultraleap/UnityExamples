from functools import reduce
import operator
from math import sqrt

def dotProduct(A, B):
    return reduce(operator.add, map(operator.mul, A, B))


def vectorAdd(A, B):
    return map(operator.add, A, B)


def vectorSubtract(A, B):
    return map(operator.sub, A, B)


def scalarMultiply(scalar, vector):
    return map(lambda v: v*scalar, vector)


def vectorNormalize(A):
    length = sqrt(reduce(operator.add, map(lambda x:x*x, A)))
    return scalarMultiply(1/length, A)
