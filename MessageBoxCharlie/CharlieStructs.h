#pragma once
struct CHARLIE_ALPHA 
{
	int id;
	char* message;
};

struct CHARLIE_BRAVO
{
	int id;
	struct CHARLIE_ALPHA *charlieAlpha;
};