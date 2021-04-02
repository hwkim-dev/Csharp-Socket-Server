#pragma once
#include <string>
//dllexport는 외부에서 접근이 가능하게함
//dllimport는 함수 또는 변수가져오기를 최적화
#ifdef LoginLibrary_EXPORTS
#define LoginLibrary_API __declspec(dllexport)
#else
#define LoginLibrary_API __declspec(dllimport)
#endif

extern "C" LoginLibrary_API bool signUp(
	const std::string id, const std::string pw,
	const std::string email, const std::string nickname);

//가입할때 아이디 중복 체크
extern "C" LoginLibrary_API bool id_Overlap(
	const std::string id);

//로그인
//아이디는 중복불가 -> 즉 기본키
extern "C" LoginLibrary_API bool login(
	const std::string id, const std::string pw);


extern "C" LoginLibrary_API bool delete_Account(
	const std::string id);

extern "C" LoginLibrary_API bool change_ID(
	const std::string id);

extern "C" LoginLibrary_API bool change_PW(
	const std::string id, const std::string PW);

// 1.존재하는계정인가?
// 2.존재한다면 이메일로 번호 보내기
extern "C" LoginLibrary_API bool Find_PW_Reset(
	const std::string id, const std::string email);
//이메일로 보낸 (랜덤)번호와 동일한가..??
extern "C" LoginLibrary_API bool isInputCorrect(
	const std::string input);

extern "C" LoginLibrary_API bool Find_ID(
	const std::string email);
