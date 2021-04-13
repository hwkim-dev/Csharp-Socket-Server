#pragma once

#ifdef Key_Table_EXPORTS
#define Key_Table_API __declspec(dllexport)
#else
#define Key_Table_API __declspec(dllimport)
#endif

#include <mutex>

namespace Key_Tabledll
{
	class Key_Table
	{
	private:
		
	public:
		static Key_Table_API void booting();

		static Key_Table_API __int8 add_Item(int verti_num);

		static Key_Table_API int find(unsigned __int8 _cursor);

		static Key_Table_API void Destroy();
	};
}

//extern "C" Key_Table_API void booting();
//
//extern "C" Key_Table_API __int8 add_Item(int verti_num);
//
//extern "C" Key_Table_API int find(unsigned __int8 _cursor);
//
//extern "C" Key_Table_API void Destroy();
