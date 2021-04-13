#pragma once

#pragma once

#ifdef Email_Vertify_Table_API
#define Email_Vertify_Table_API __declspec(dllexport)
#else
#define Email_Vertify_Table_API __declspec(dllimport)
#endif

#include <mutex>

namespace Key_Tabledll
{
	class Email_Table
	{
	private:

	public:
		static Email_Vertify_Table_API void booting();

		static Email_Vertify_Table_API __int8 add_Item(int verti_num);

		static Email_Vertify_Table_API int find(unsigned __int8 _cursor);

		static Email_Vertify_Table_API void Destroy();
	};
}

//extern "C" Key_Table_API void booting();
//
//extern "C" Key_Table_API __int8 add_Item(int verti_num);
//
//extern "C" Key_Table_API int find(unsigned __int8 _cursor);
//
//extern "C" Key_Table_API void Destroy();
