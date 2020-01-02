#pragma once
#include <Windows.h>

class XamlWindow
{
public:
    XamlWindow();
    virtual ~XamlWindow();

    
    static HWND hWnd;
    static HWND childhWnd;

protected:


    static void Paint(HWND window);
    static void CreateChildWindow();
    static void Size();

    static XamlWindow* this_from_hwnd(HWND window);
    static LRESULT __stdcall window_proc(HWND window, UINT message, WPARAM wparam, LPARAM lparam);

    bool initialized = false;

    UINT window_width, window_height;
};