// GPUProcessor.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "GPUProcessor.h"

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Default constructor. </summary>
///
/// <remarks>	Angel, 7/05/2013. </remarks>
////////////////////////////////////////////////////////////////////////////////////////////////////

CGPUProcessor::CGPUProcessor() {
	return;
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Executes the code on GPU operation. </summary>
///
/// <remarks>	Angel, 7/05/2013. </remarks>
///
/// <param name="returnSink">	The return sink. </param>
/// <param name="vC">		 	[in,out] The v c. </param>
/// <param name="vA">		 	The v a. </param>
/// <param name="vB">		 	The v b. </param>
/// <param name="M">		 	The int to process. </param>
/// <param name="N">		 	The int to process. </param>
/// <param name="W">		 	The width. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void CGPUProcessor::ExecuteCodeOnGpu(const void* returnSink, std::vector<float>& vC, const std::vector<float>& vA, const std::vector<float>& vB, int M, int N,int W) {
	array_view<const float, 2> a(M, W, vA);
	array_view<const float, 2> b(W, N, vB);
	array_view<float, 2> c(M, N, vC);

	c.discard_data(); 

	parallel_for_each(c.extent, [=](index<2> idx) restrict(amp) {
		auto row = idx[0]; 
		auto col = idx[1];
		auto sum = 0.0f;
		for(auto i = 0; i < W; i++)
			sum += a(row, i) * b(i, col);
		c[idx] = sum;
	}); 

	c.synchronize();

	((ptrClrDelegate) returnSink)(vC.data(), vC.size(), MathOperation::MatricesMultiplication);
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Calculates the option price with black scholes. </summary>
///
/// <remarks>	Angel, 7/05/2013. </remarks>
///
/// <param name="returnSink">	The return sink. </param>
/// <param name="vC">		 	[in,out] The v c. </param>
/// <param name="vA">		 	The v a. </param>
/// <param name="vB">		 	The v b. </param>
/// <param name="rate">		 	The rate. </param>
/// <param name="volatility">	The volatility. </param>
/// <param name="maturity">  	The maturity. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void CGPUProcessor::CalculateOptionPriceWithBlackScholes(const void* returnSink, std::vector<float>& vC, const std::vector<float>& vA, const std::vector<float>& vB, const float& rate, const float& volatility, const float& maturity) {
	if (vA.size() == vB.size()) {
		auto arrayLength = vA.size(); 
		array_view<float, 1> c(arrayLength, vC);
		array_view<const float, 1> a(arrayLength, vA);
		array_view<const float, 1> b(arrayLength, vB);

		c.discard_data();

		auto normalDist = [&](const float& value) restrict(amp) -> float {return (1.0f / fast_math::sqrt(2.0f * (float) PI)) * fast_math::exp(-0.5f * value * value);};

		parallel_for_each(concurrency::extent<1>(arrayLength), [=](index<1> idx) restrict(amp) {
			auto s = a(idx) * fast_math::exp(-rate * b(idx));
			auto d1 = b(idx) * a(idx) * fast_math::exp(-rate * b(idx));
			c[idx] = d1 / s;
		}); 

		c.synchronize();

		((ptrClrDelegate) returnSink)(vC.data(), vC.size(), MathOperation::CalculateOptionPriceWithBlackScholes);
	}
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Executes the in GPU operation. </summary>
///
/// <remarks>	Angel, 7/05/2013. </remarks>
///
/// <param name="matrixA">			[in,out] If non-null, the matrix a. </param>
/// <param name="matrixB">			[in,out] If non-null, the matrix b. </param>
/// <param name="matrixALength">	Length of the matrix a. </param>
/// <param name="matrixBLength">	Length of the matrix b. </param>
/// <param name="M">				The int to process. </param>
/// <param name="N">				The int to process. </param>
/// <param name="W">				The width. </param>
/// <param name="returnSink">   	The return sink. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void RunInGpu(float* matrixA,  float* matrixB, int matrixALength, int matrixBLength, int M, int N, int W,  const void* returnSink) {
	CGPUProcessor gpuProcessor;
	std::vector<float> vC (M * N);
	std::vector<float> vA (matrixA, matrixA + matrixALength);
	std::vector<float> vB (matrixB, matrixB + matrixBLength);
	gpuProcessor.ExecuteCodeOnGpu(returnSink, vC, vA, vB, M, N, W);
}

////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>	Calculates the option price with black scholes in GPU. </summary>
///
/// <remarks>	Angel, 7/05/2013. </remarks>
///
/// <param name="matrixA">			[in,out] If non-null, the matrix a. </param>
/// <param name="matrixB">			[in,out] If non-null, the matrix b. </param>
/// <param name="matrixALength">	Length of the matrix a. </param>
/// <param name="matrixBLength">	Length of the matrix b. </param>
/// <param name="rate">				The rate. </param>
/// <param name="volatility">   	The volatility. </param>
/// <param name="maturity">			The maturity. </param>
/// <param name="returnSink">   	The return sink. </param>
////////////////////////////////////////////////////////////////////////////////////////////////////

void CalculateOptionPriceWithBlackScholesInGpu(float* matrixA, float* matrixB, int matrixALength, int matrixBLength, float rate, float volatility, float maturity, const void* returnSink) {
	if (matrixALength == matrixBLength) { // Matrices size have to match because matrixA is for spot (underlying) price and matrixB strike (exercise) price
		CGPUProcessor gpuProcessor;
		auto arrayLength = matrixALength;
		std::vector<float> vC (arrayLength);
		std::vector<float> vA (matrixA, matrixA + arrayLength);
		std::vector<float> vB (matrixB, matrixB + arrayLength);
		gpuProcessor.CalculateOptionPriceWithBlackScholes(returnSink, vC, vB, vB, rate, volatility, maturity);
	}
}


/* C++ Code 9.2: Calculating the delta of the Black Scholes call option price */