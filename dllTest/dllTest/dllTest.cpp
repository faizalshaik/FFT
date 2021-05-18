// dllTest.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <tchar.h>
#include <Windows.h>

typedef void(__stdcall* onFinished)(bool bError, LPCTSTR strError);
typedef void(__stdcall* onProgress)(int total, int processed);

void __stdcall Finished(bool bError, LPCTSTR strError)
{

}
void __stdcall onProgressed(int t, int c)
{
    int percent = c * 100 / t;
    std::cout << "percent=" << percent << std::endl;
}
typedef bool(__stdcall* pStart)(LPCTSTR strFile, int channelCount, LPCTSTR strOutFile, int nThreads);
typedef bool(__stdcall* pCalcFFT)(LPSAFEARRAY* samples, LPSAFEARRAY* decbls);

int main()
{
    std::cout << "Hello World!\n";
    HMODULE hMod = LoadLibrary(_T("fftlib.dll"));
    pStart start = (pStart)GetProcAddress(hMod, "Start");
    //pCalcFFT calcFFT = (pCalcFFT)GetProcAddress(hMod, "CalcFFT");
    //calcFFT(NULL, NULL);

    start(_T("d:\\Binary File.bin"), 4, _T("d:\\1.bin"), 400);


    //CGlobal::instance()->setScale(0, 0.0, 3.0);
    //CGlobal::instance()->start(_T("d:\\Sample FullBlock.bin"), Finished);
    MessageBox(0, 0, 0, 0);
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
