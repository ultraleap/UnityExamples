
#ifndef SENSATIONCORE_EXPORT_H
#define SENSATIONCORE_EXPORT_H

#ifdef SENSATIONCORE_STATIC_DEFINE
#  define SENSATIONCORE_EXPORT
#  define SENSATIONCORE_NO_EXPORT
#else
#  ifndef SENSATIONCORE_EXPORT
#    ifdef SensationCore_EXPORTS
        /* We are building this library */
#      define SENSATIONCORE_EXPORT __attribute__((visibility("default")))
#    else
        /* We are using this library */
#      define SENSATIONCORE_EXPORT __attribute__((visibility("default")))
#    endif
#  endif

#  ifndef SENSATIONCORE_NO_EXPORT
#    define SENSATIONCORE_NO_EXPORT __attribute__((visibility("hidden")))
#  endif
#endif

#ifndef SENSATIONCORE_DEPRECATED
#  define SENSATIONCORE_DEPRECATED __attribute__ ((__deprecated__))
#endif

#ifndef SENSATIONCORE_DEPRECATED_EXPORT
#  define SENSATIONCORE_DEPRECATED_EXPORT SENSATIONCORE_EXPORT SENSATIONCORE_DEPRECATED
#endif

#ifndef SENSATIONCORE_DEPRECATED_NO_EXPORT
#  define SENSATIONCORE_DEPRECATED_NO_EXPORT SENSATIONCORE_NO_EXPORT SENSATIONCORE_DEPRECATED
#endif

#if 0 /* DEFINE_NO_DEPRECATED */
#  ifndef SENSATIONCORE_NO_DEPRECATED
#    define SENSATIONCORE_NO_DEPRECATED
#  endif
#endif

#endif /* SENSATIONCORE_EXPORT_H */
