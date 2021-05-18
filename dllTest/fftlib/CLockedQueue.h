#pragma once
#include <Windows.h>
#include <vector>

template<class T>
class CLockedQueue
{
protected:
	CRITICAL_SECTION _lock;
	std::vector<T*>   _data;
public:
	CLockedQueue()
	{
		InitializeCriticalSection(&_lock);
	}
	~CLockedQueue()
	{
		DeleteCriticalSection(&_lock);
	}
	void append(T* v)
	{
		EnterCriticalSection(&_lock);
		_data.push_back(v);
		LeaveCriticalSection(&_lock);
	}
	T* take()
	{
		T* r = NULL;
		EnterCriticalSection(&_lock);
		if (_data.size() > 0)
		{
			r = _data[0];
			_data.erase(_data.begin());
		}
		LeaveCriticalSection(&_lock);
		return r;
	}
	int size() {
		int size = 0;
		EnterCriticalSection(&_lock);
		size =  _data.size();
		LeaveCriticalSection(&_lock);
		return size;
	}

	void takeAll(std::vector<T*>& out)
	{
		EnterCriticalSection(&_lock);
		for (int i = 0; i < _data.size(); i++)
			out.push_back(_data[i]);
		_data.clear();
		LeaveCriticalSection(&_lock);
	}

	void clear()
	{
		EnterCriticalSection(&_lock);
		for (int i = 0; i < _data.size(); i++)
			delete _data[i];
		_data.clear();
		LeaveCriticalSection(&_lock);
	}
};

