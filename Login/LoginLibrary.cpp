#include<winsock2.h>
#include<iostream>
#include<string>
#include<stdio.h>
#include<mysql.h>
#include "pch.h"

#pragma comment(lib,"libmysql.lib")
#pragma comment(lib,"ws2_32.lib")

static class sql
{
	MYSQL* connection = NULL, conn;
	MYSQL_RES* sql_result;
	MYSQL_ROW sql_row;
	int query_stat;

	int main() {
		/*MYSQL *connection = NULL, conn;
		MYSQL_RES *sql_result;
		MYSQL_ROW sql_row;
		int query_stat;*/

		mysql_init(&conn);

		//포트는 sql서버 설치시 지정하게되는데, 그냥 넘기면 기본값이 3306이다.
		connection = mysql_real_connect(&conn, "127.0.0.1", "root", "dbname", "test", 3306, (char*)NULL, 0);

		if (connection == NULL) {
			printf("con error");
			return 1;
		}

		//쿼리:한글사용위해 
		mysql_query(connection, "set session character_set_connection=euckr;");
		mysql_query(connection, "set session character_set_results=euckr;");
		mysql_query(connection, "set session character_set_client=euckr;");
		
		/*
		//쿼리:테이블생성
		query_stat = mysql_query(connection,
			"CREATE TABLE user(key int not null auto_increment primary key, id varchar(20),pw varchar(30))");
		if (query_stat != 0)
		{
			printf("error : %s ", mysql_error(&conn));
			return 1;
		}

		// 쿼리:셀렉트
		query_stat = mysql_query(connection, "select id, pw from user");
		if (query_stat != 0)
		{
			fprintf(stderr, "Mysql query error : %s", mysql_error(&conn));
			return 1;
		}

		// 결과출력
		sql_result = mysql_store_result(connection);
		while ((sql_row = mysql_fetch_row(sql_result)) != NULL)
		{
			printf("%s %s\n", sql_row[0], sql_row[1]);
		}
		mysql_free_result(sql_result);


		*/
		signUp("z", "z", "z", "z");
		// DB 연결닫기
		mysql_close(connection);
		return 0;
	}

	bool signUp(const std::string id, const std::string pw, const std::string email, const std::string nickname)
	{
		// 쿼리:레코드삽입
		char query[255];
		sprintf(query, "insert into user(id, pw, email, nickname) values(\'id\',\'pw\',\'email\',\'nickname\')");
		query_stat = mysql_query(connection, query);
		if (query_stat != 0) {
			printf("error : %s", mysql_error(&conn));
			return 1;
		}
	}

	//가입할때 아이디 중복 체크
	bool id_Overlap(
		const std::string id)
	{

	}


	//로그인
	//아이디는 중복불가 -> 즉 기본키
	bool login(
		const std::string id, const std::string pw)
	{

	}


	bool delete_Account(
		const std::string id)
	{

	}

	bool change_ID(
		const std::string id)
	{

	}

	bool change_PW(
		const std::string id, const std::string PW)
	{

	}

	// 1.존재하는계정인가?
	// 2.존재한다면 이메일로 번호 보내기
	bool Find_PW_Reset(
		const std::string id, const std::string email)
	{

	}
	//이메일로 보낸 (랜덤)번호와 동일한가..??
	bool isInputCorrect(
		const std::string input)
	{

	}

	bool Find_ID(
		const std::string email)
	{

	}
};




//새로 가입했을때 SQL문
//INSERT INTO ACCOUNT(ID, PW, EMAIL, NICKNAME)
//VALUES(, , , ){

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

