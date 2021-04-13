#include "pch.h"
#include "Email_Verify_Table.h"

#include <iostream>

#define TIMER 3
#define SIZE 100

namespace Key_Tabledll
{
	int** table;
	const __int8 FAIL{ -1 };
	const __int8 RESETVAL{ 0 };
	__int8 cursor{ 0 };
	__int8 time{ 0 };
	std::mutex mtx;
	__int8 add_Item(int verti_num)
	{
		mtx.lock();
		try
		{
			table[time][cursor] = verti_num;
			return cursor;
		}
		catch (int wrong)
		{
			return FAIL;
		}
		mtx.unlock();
		return 1;
	}

	int find(unsigned __int8 _cursor)
	{
		try
		{
			return (int)table[time][_cursor];
		}
		catch (char e)
		{
			//return Null
			return '\0';
		}
	}



	void Destroy()
	{
		try
		{
			do
			{
				//O(N)시간 소요
				table[time][cursor] = RESETVAL;
				--cursor;
			} while (cursor >= 0);
			//다음에 사용하기 위해 cursor를 0으로 만들어줌
			++cursor;
		}
		catch (char e)
		{
			std::cout << "Destroy Error Occurred" << std::endl;
		}

		/*Destory()가 실행된후 time을 옮겨야
		삭제되는 도중 삽입되는 오류를 방지
		3분에서 4분으로 바뀔때 실행
		시간 + 1
		3으로 나눠서 3을 초과하지 못하게 맞음*/
		time = (__int8)((++time) % 3);
	}
}