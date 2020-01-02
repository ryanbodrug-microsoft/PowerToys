#include "pch.h"
#include "XamlWindow.h"

#include <windows.h>
#include <stdlib.h>
#include <string.h>

#include <unknwn.h>
#include <winrt/Windows.system.h>
#include <winrt/windows.ui.xaml.hosting.h>
#include <windows.ui.xaml.hosting.desktopwindowxamlsource.h>
#include <winrt/windows.ui.xaml.controls.h>
#include <winrt/Windows.ui.xaml.media.h>
#include <winrt/Windows.Foundation.Collections.h>

using namespace winrt;
using namespace Windows::UI;
using namespace Windows::UI::Composition;
using namespace Windows::UI::Xaml::Hosting;
using namespace Windows::Foundation::Numerics;

extern "C" IMAGE_DOS_HEADER __ImageBase;

HWND XamlWindow::hWnd = 0;
HWND XamlWindow::childhWnd = 0;

XamlWindow::XamlWindow()
{
    HINSTANCE hInstance = reinterpret_cast<HINSTANCE>(&__ImageBase);
    // The main window class name.
    const wchar_t szWindowClass[] = L"Win32DesktopApp";
    WNDCLASSEX windowClass = {};

    windowClass.cbSize = sizeof(WNDCLASSEX);
    windowClass.lpfnWndProc = window_proc;
    windowClass.hInstance = hInstance;
    ;
    windowClass.lpszClassName = szWindowClass;
    windowClass.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);

    windowClass.hIconSm = LoadIcon(windowClass.hInstance, IDI_APPLICATION);

    if (RegisterClassEx(&windowClass) == NULL)
    {
        MessageBox(NULL, L"Windows registration failed!", L"Error", NULL);
        return;
    }

    hWnd = CreateWindow(
        szWindowClass,
        L"Windows c++ Win32 Desktop App",
        WS_OVERLAPPEDWINDOW | WS_VISIBLE,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        CW_USEDEFAULT,
        NULL,
        NULL,
        hInstance,
        NULL);
    if (hWnd == NULL)
    {
        MessageBox(NULL, L"Call to CreateWindow failed!", L"Error", NULL);
        return;
    }

    //XAML Island section

    // The call to winrt::init_apartment initializes COM; by default, in a multithreaded apartment.
    winrt::init_apartment(apartment_type::single_threaded);

    // Initialize the Xaml Framework's corewindow for current thread
    WindowsXamlManager winxamlmanager = WindowsXamlManager::InitializeForCurrentThread();

    // This DesktopWindowXamlSource is the object that enables a non-UWP desktop application
    // to host UWP controls in any UI element that is associated with a window handle (HWND).
    DesktopWindowXamlSource desktopSource;
    // Get handle to corewindow
    auto interop = desktopSource.as<IDesktopWindowXamlSourceNative>();
    // Parent the DesktopWindowXamlSource object to current window
    check_hresult(interop->AttachToWindow(hWnd));

    // This Hwnd will be the window handler for the Xaml Island: A child window that contains Xaml.
    HWND hWndXamlIsland = nullptr;
    // Get the new child window's hwnd
    interop->get_WindowHandle(&hWndXamlIsland);
    // Update the xaml island window size becuase initially is 0,0
    SetWindowPos(hWndXamlIsland, 0, 200, 100, 800, 200, SWP_SHOWWINDOW);

    //Creating the Xaml content
    Windows::UI::Xaml::Controls::StackPanel xamlContainer;
    xamlContainer.Background(Windows::UI::Xaml::Media::SolidColorBrush{ Windows::UI::Colors::LightGray() });

    Windows::UI::Xaml::Controls::TextBlock tb;
    tb.Text(L"Hello World from Xaml Islands!");
    tb.VerticalAlignment(Windows::UI::Xaml::VerticalAlignment::Center);
    tb.HorizontalAlignment(Windows::UI::Xaml::HorizontalAlignment::Center);
    tb.FontSize(48);

    xamlContainer.Children().Append(tb);
    xamlContainer.UpdateLayout();
    desktopSource.Content(xamlContainer);

    //End XAML Island section

    ShowWindow(hWnd, SW_SHOWNORMAL);
    UpdateWindow(hWnd);

    //Message loop:
    MSG msg = {};
    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return;
}

XamlWindow::~XamlWindow()
{
    ShowWindow(childhWnd, SW_HIDE);
    ShowWindow(hWnd, SW_HIDE);
    DestroyWindow(childhWnd);
    DestroyWindow(hWnd);
}

XamlWindow* XamlWindow::this_from_hwnd(HWND window)
{
    return reinterpret_cast<XamlWindow*>(GetWindowLongPtr(window, GWLP_USERDATA));
}

void XamlWindow::Paint(HWND window)
{
    if (window == hWnd)
    {
        PAINTSTRUCT ps;
        HDC hdc;
        wchar_t greeting[] = L"Hello World in Win32!";
        hdc = BeginPaint(hWnd, &ps);
        TextOut(hdc, 300, 5, greeting, wcslen(greeting));
        EndPaint(hWnd, &ps);
    }
}

void XamlWindow::CreateChildWindow()
{
    HINSTANCE hInstance = reinterpret_cast<HINSTANCE>(&__ImageBase);
    childhWnd = CreateWindowEx(0, L"ChildWClass", NULL, WS_CHILD | WS_BORDER, 0, 0, 0, 0, hWnd, NULL, hInstance, NULL);
    return;
}

void XamlWindow::Size()
{
    // Get the dimensions of the main window's client
    // area, and enumerate the child windows. Pass the
    // dimensions to the child windows during enumeration.
    RECT rcClient;
    GetClientRect(hWnd, &rcClient);
    MoveWindow(childhWnd, 200, 200, 400, 500, TRUE);
    ShowWindow(childhWnd, SW_SHOW);

    return;
}

LRESULT CALLBACK XamlWindow::window_proc(HWND window, UINT messageCode, WPARAM wParam, LPARAM lParam)
{


    switch (messageCode)
    {
    case WM_PAINT:
           XamlWindow::Paint(window);
        break;
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
        //creating main window
    case WM_CREATE:
        XamlWindow::CreateChildWindow();
        break;
        // main window changed size
    case WM_SIZE:
        XamlWindow::Size();
        break;
        // Process other messages.

    default:
        return DefWindowProc(window, messageCode, wParam, lParam);
        break;
    }

    return 0;
}