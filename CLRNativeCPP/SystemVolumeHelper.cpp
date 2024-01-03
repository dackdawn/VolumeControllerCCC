#include "pch.h"
#include "../SystemVolumeHelper/SystemVolumeHelper.h"
#include "SystemVolumeHelper.h"
#include <iostream>

namespace CLRNativeCPP
{
	SystemVolumeHelper::SystemVolumeHelper()
	{
		try
		{
			Init();
		}
		catch (std::exception e)
		{
			handler = nullptr;
			throw;
		}
	}

	SystemVolumeHelper::~SystemVolumeHelper()
	{
		if (handler)
			delete handler;
	}

	int SystemVolumeHelper::get_volume()
	{
		return handler->GetVolume();
	}

	void SystemVolumeHelper::set_volume(int new_volume)
	{
		handler->SetVolume(new_volume);
	}

	void SystemVolumeHelper::Init()
	{
		handler = new SystemVolume::SystemVolume();
	}


}
