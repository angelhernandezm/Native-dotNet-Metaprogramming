// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the GPUPROCESSOR_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// GPUPROCESSOR_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef GPUPROCESSOR_EXPORTS
#define GPUPROCESSOR_API __declspec(dllexport)
#else
#define GPUPROCESSOR_API __declspec(dllimport)
#endif

// This class is exported from the GPUProcessor.dll
class GPUPROCESSOR_API CGPUProcessor {
public:
	CGPUProcessor(void);
	void ExecuteCodeOnGpu(const void* returnSink, std::vector<float>& vC, const std::vector<float>& vA, const std::vector<float>& vB, int M, int N, int W);
	void CalculateOptionPriceWithBlackScholes(const void* returnSink, std::vector<float>& vC, const std::vector<float>& vA, const std::vector<float>& vB, const float& rate, const float& volatility, const float& maturity);
};

extern "C"{
	GPUPROCESSOR_API void RunInGpu(float* matrixA, float* matrixB, int matrixALength, int matrixBLength, int M, int N, int W, const void* returnSink);
	GPUPROCESSOR_API void CalculateOptionPriceWithBlackScholesInGpu(float* matrixA, float* matrixB, int matrixALength, int matrixBLength, float rate, float volatility, float maturity, const void* returnSink);
}
