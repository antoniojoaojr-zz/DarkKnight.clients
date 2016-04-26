#include "Connection.h"

extern "C" Connection* create_object() {
	return new Connection;
}

extern "C" void destroy_object(Connection* object) {
	delete object;
}


Connection::Connection()
{
}


Connection::~Connection()
{
}
