// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>
#include <iostream>
#include <amp.h>
#include <vector>
#include <sstream> 
#include <amp_math.h>

using namespace std;
using namespace concurrency;
using namespace concurrency::precise_math;

typedef void (*ptrClrDelegate) (void* results, int itemCount, int operation);

typedef enum MathOperation {
	MatricesMultiplication, 
	CalculateOptionPriceWithBlackScholes
};

#ifndef PI 
#define PI 3.1416
#endif



// TODO: reference additional headers your program requires here
