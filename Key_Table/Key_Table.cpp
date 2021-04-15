#include "pch.h"
#include "Key_Table.h"

#define TIMER 3
#define SIZE 100
#define KEYSIZE 5

namespace Key_Tabledll
{
	int** table = new int* [TIMER];

	int key[KEYSIZE];
	const __int8 FAIL{ -1 };
	const __int8 RESETVAL{ 0 };
	__int8 cursor = 0;
	__int8 time = 0;
	std::mutex mtx;


	void booting()
	{
		std::cout << "Loading Key_Table..." << std::endl;
		for (int c = 0; c < TIMER; ++c)
		{
			table[c] = new int[SIZE];
		}
		cursor = 0;
		time = 0;
	}

	__int8 add_Item(int verti_num)
	{
		//mtx.lock();
		try
		{
			table[time][cursor] = verti_num;
			//mtx.unlock();
			return ++cursor - 1;
		}
		catch (int wrong)
		{
			std::cout << "ERROR!" << std::endl;
			return FAIL;
		}

	}

	int find(unsigned char _cursor)
	{
		try
		{
			return table[time][_cursor];
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