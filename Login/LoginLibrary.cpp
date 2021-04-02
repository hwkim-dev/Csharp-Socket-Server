#include "pch.h" // use stdafx.h in Visual Studio 2017 and earlier
#include <utility>
#include <limits.h>
#include "LoginLibrary.h"
#include <mysql.h>


//새로 가입했을때 SQL문
//INSERT INTO ACCOUNT(ID, PW, EMAIL, NICKNAME)
//VALUES(, , , );

//아이디 찾기 SQL문 -> 아이디중복체크 / 로그인
//SELECT ID
//FROM ACCOUNT
//WHERE ID = 아이디입력

//아이디 찾기 SQL문 -> EMAIL로 아이디 찾을때/이메일로 보낼때
//SELECT ID
//FROM ACCOUNT
//WHERE EMAIL = 입력된이메일

//패스워드 찾기 SQL문 -> 로그인
// SELECT PW
// FROM ACCOUNT
// WHERE PW = 비밀번호입력

//아이디 바꾸기 SQL문 -> 
//UPDATE ACCOUNT
//SET ID = 새아이디
//WHERE ID = 과거아이디

//비밀번호 바꾸기 SQL문 ->
//UPDATE ACCOUNT
//SET PW = 새아이디
//WHERE PW = 과거아이디

//계졍삭제(하기전에 아이디비밀번호 맞는지부터 체크)
//DELETE FROM ACCOUNT
//WHERE ID = 입력한아이디

