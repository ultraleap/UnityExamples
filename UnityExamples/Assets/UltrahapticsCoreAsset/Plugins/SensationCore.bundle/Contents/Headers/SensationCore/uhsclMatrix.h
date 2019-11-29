#pragma once

#include "uhsclVector.h"

#include "pack.hpp"

#ifdef __cplusplus
extern "C" {
#endif


PACK(typedef struct
{
    double rowMajor[16];
}) uhsclMatrix4_t;

uhsclMatrix4_t getIdentityMatrix4();
void insertVectorIntoMatrixAtColumn(const uhsclVector3_t &v, uhsclMatrix4_t &m, int colIdx);
uhsclMatrix4_t composeMatrixFromVectors(const uhsclVector3_t &x,
                                        const uhsclVector3_t &y,
                                        const uhsclVector3_t &z,
                                        const uhsclVector3_t &o);

#ifdef __cplusplus
}

#endif
