#include "MessageBoxBeta.h"
#pragma once
#ifdef __cplusplus
extern "C" {
#endif
	extern __declspec(dllimport) MessageBoxBeta^ CreateMessageBoxBeta();
	extern __declspec(dllimport) void DisposeMessageBoxBeta(MessageBoxBeta^ messageBoxBeta);
	extern __declspec(dllimport) void ShowMessageBoxBeta(MessageBoxBeta^ messageBoxBeta);
#ifdef __cplusplus
}
#endif