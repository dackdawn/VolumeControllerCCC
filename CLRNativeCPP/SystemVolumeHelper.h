#pragma once
#include "../SystemVolumeHelper/SystemVolumeHelper.h"
namespace CLRNativeCPP
{
	public ref class SystemVolumeHelper
	{
		SystemVolume::SystemVolume* handler;
		void Init();
	public:
		SystemVolumeHelper();
		~SystemVolumeHelper();
		auto get_volume() -> int;
		auto set_volume(int) -> void;
	};
}
