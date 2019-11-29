#pragma once

#include "pack.hpp"

#ifdef __cplusplus
extern "C" {
#endif


PACK(typedef struct
{
    double x, y, z;
}) uhsclVector3_t;

PACK(typedef struct
 {
     double x, y, z, w;
 }) uhsclVector4_t;

int isValidVector3_t(uhsclVector3_t focalPoint);
int isValidVector4_t(uhsclVector4_t focalPoint);

uhsclVector3_t getInvalidUhsclVector3_t(void);
uhsclVector4_t getInvalidUhsclVector4_t(void);

uhsclVector3_t parseUhsclVector3(const char *string);

#ifdef __cplusplus
}

uhsclVector3_t operator+(uhsclVector3_t const &lhs, uhsclVector3_t const &rhs);
uhsclVector3_t operator-(uhsclVector3_t const &lhs, uhsclVector3_t const &rhs);
double lengthVector3_t(uhsclVector3_t const &v);
double dotProduct(uhsclVector3_t const &lhs, uhsclVector3_t const &rhs);
uhsclVector3_t crossProduct(uhsclVector3_t const &lhs, uhsclVector3_t const &rhs);
uhsclVector3_t operator*(const uhsclVector3_t &lhs,const double &scalar);
uhsclVector3_t operator/(const uhsclVector3_t &lhs,const double &scalar);
uhsclVector3_t normalize(uhsclVector3_t const &v);


#endif
