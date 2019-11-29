#pragma once

// This header is used to control decoration of public functions.
//
// On Windows when building a DLL and .lib functions must be decorated with `__declspec(dllexport)`
// while building the library, and `__declspec(dllimport)` when another program is using the library.
//
// The `__declspec(dllexport)` causes MSVC to actually export the functions and generate the .lib
// file. An alternative to using `__declspec(dllexport)` is to list exported functions manually in
// a .def file. The `__declspec(dllimport)` is an optimisation that removes one `jmp` for each function
// call.
//
// This is explained reasonably well here: https://msdn.microsoft.com/en-us/library/aa271769(v=vs.60).aspx
//
// On non-Windows platforms we use the decoration macros to make the symbols visible. By default our build
// system sets all symbols to be invisible. `__attribute__((visibility("default"))` overrides that
// and allows them to be used by programs that link with this library.

// UH_API_CLASS_DECORATION is applied to classes and global functions.
// UH_API_DECORATION is applied to class methods.

// NO_DECORATION is used when generating documentation and for SWIG.
#ifndef NO_DECORATION

    #ifdef _WIN32

        // Class methods do not need to be declared __declspec(...).
        // The decoration is only done on the class itself.
        #ifdef UH_LIBRARY_EXPORT

            #define UH_API_CLASS_DECORATION __declspec(dllexport)
            #if RESEARCH_BUILD
                #define UH_RESEARCH_API_CLASS_DECORATION __declspec(dllexport)
            #endif

        #else

            #define UH_API_CLASS_DECORATION __declspec(dllimport)
            #define UH_RESEARCH_API_CLASS_DECORATION __declspec(dllimport)

        #endif

    #else

        #define UH_API_DECORATION __attribute__((visibility("default")))
        #define UH_API_CLASS_DECORATION __attribute__((visibility("default")))

        #if RESEARCH_BUILD
            #define UH_RESEARCH_API_DECORATION __attribute__((visibility("default")))
            #define UH_RESEARCH_API_CLASS_DECORATION __attribute__((visibility("default")))
        #endif

    #endif
#endif

// Default to undecorated.
#ifndef UH_API_DECORATION
    #define UH_API_DECORATION
#endif
#ifndef UH_API_CLASS_DECORATION
    #define UH_API_CLASS_DECORATION
#endif
#ifndef UH_RESEARCH_API_DECORATION
    #define UH_RESEARCH_API_DECORATION
#endif
#ifndef UH_RESEARCH_API_CLASS_DECORATION
    #define UH_RESEARCH_API_CLASS_DECORATION
#endif
