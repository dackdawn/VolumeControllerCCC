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
        /**��ʼ������*/
        void Init();
        /**�رշ��� �ͷ���Դ*/
        void Close() const;
        /**��������*/
        void SetVolume(int volume);
        /**��ȡϵͳ����*/
        int GetVolume();
        /**����*/
        void Mute();
        /**�������*/
        void UnMute();
    };
} // namespace SystemVolume
#endif