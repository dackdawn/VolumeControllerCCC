#pragma once
#ifndef SYSTEM_VOLUME_HELPER_H
#define SYSTEM_VOLUME_HELPER_H
#include <Windows.h>
#include <mmdeviceapi.h>
#include <endpointvolume.h>
#include <Audioclient.h>

// #ifdef SYSTEM_VOLUME_DDLEXPORT // SYSTEM_VOLUME_DDLEXPORT
// #define SYSTEM_DLLEXPORTAPI extern "C" __declspec(dllexport)
// #else
// #define SYSTEM_DLLEXPORTAPI __declspec(dllimport)
// #endif //! SYSTEM_DLLEXPORT



namespace SystemVolume
{

    class SystemVolume final
    {
    public:
        explicit SystemVolume();
        virtual ~SystemVolume();

    private:
        HRESULT hr{};
        IMMDeviceEnumerator* pDeviceEnumerator = nullptr;
        IMMDevice* pDevice = nullptr;
        IAudioEndpointVolume* pAudioEndpointVolume = nullptr;
        IAudioClient* pAudioClient = nullptr;

    public:
        /**初始化服务*/
        void Init();
        /**关闭服务 释放资源*/
        void Close() const;
        /**设置音量*/
        void SetVolume(int volume);
        /**获取系统音量*/
        int GetVolume();
        /**静音*/
        void Mute();
        /**解除静音*/
        void UnMute();
    };
} // namespace SystemVolume
#endif