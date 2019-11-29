#pragma once

#if defined(__GNUC__)
#define PACK( ... ) __VA_ARGS__ __attribute__((__packed__))
#elif defined(_MSC_VER)
#define PACK( ... ) __pragma( pack(push, 1) ) __VA_ARGS__ __pragma( pack(pop) )
#endif
