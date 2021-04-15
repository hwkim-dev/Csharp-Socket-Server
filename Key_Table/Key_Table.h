#pragma once
#ifndef KEY_TABLE_H
#define KEY_TABLE_H

#include <Mutex>
#include <iostream>

using namespace std;

#ifdef __cplusplus
extern "C" {
#endif

#ifdef BUILD_MY_DLL
#define KEY_TABLE __declspec(dllexport)
#else 
#define KEY_TABLE __declspec(dllimport)
#endif
    namespace Key_Tabledll
    {
        void booting();
        __int8 add_Item(int verti_num);
        int find(unsigned __int8 _cursor);
        void Destroy();
    }
#ifdef __cplusplus
}
#endif

#endif//end of Dll