// SystemVolumeHelper.cpp : 定义静态库的函数。
//

#include "pch.h"
#include "framework.h"
#include "SystemVolumeHelper.h"
#include <cmath>
#include <memory>
#include <string>
#include <iostream>

#pragma comment(lib, "Ole32.lib")

namespace SystemVolume
{
    SystemVolume::SystemVolume()
    {
        try
        {
            Init();

            GetVolume();
        }
        catch (std::string e)
        {

            std::cout << "SystemVolume Init error:" << e << std::endl;
        }
        catch (...)
        {
        }
    }
    SystemVolume::~SystemVolume()
    {
        Close();
    }

    void SystemVolume::Init()
    {
        hr = CoInitialize(nullptr);
        hr = CoCreateInstance(
            __uuidof(MMDeviceEnumerator), nullptr, CLSCTX_ALL, __uuidof(IMMDeviceEnumerator),
            reinterpret_cast<void**>(&pDeviceEnumerator));
        if (FAILED(hr))
            throw std::exception("InitException: pDeviceEnumerator is NULL.");
        hr = pDeviceEnumerator->GetDefaultAudioEndpoint(eRender, eMultimedia, &pDevice);
        if (FAILED(hr))
            throw std::exception("InitException: pDevice is NULL.");
        hr = pDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_ALL, nullptr,
                               reinterpret_cast<void**>(&pAudioEndpointVolume));
        if (FAILED(hr))
            throw std::exception("pDevice->Active: pAudioEndpointVolume got error.");
        hr = pDevice->Activate(__uuidof(IAudioClient), CLSCTX_ALL, nullptr, reinterpret_cast<void**>(&pAudioClient));
        if (FAILED(hr))
            throw std::exception("pDevice->Active: pAudioClient got error.");
    }

    void SystemVolume::Close() const
    {
        if (pAudioClient)
            pAudioClient->Release();
        if (pAudioEndpointVolume)
            pAudioEndpointVolume->Release();
        if (pDevice)
            pDevice->Release();
        if (pDeviceEnumerator)
            pDeviceEnumerator->Release();
        CoUninitialize();
    }

    void SystemVolume::SetVolume(int volume)
    {
        if ((volume - 100) > 0)
            volume = 100;
        else if (volume < 0)
            volume = 0;

        const float fVolume = volume / 100.0f;
        hr = pAudioEndpointVolume->SetMasterVolumeLevelScalar(fVolume, &GUID_NULL);
        if (FAILED(hr))
            throw std::exception("SetMasterVolumeLevelScalar");
    }

    int SystemVolume::GetVolume()
    {
        float volume;
        hr = pAudioEndpointVolume->GetMasterVolumeLevelScalar(&volume);
        if (FAILED(hr))
            throw std::exception("getVolume() throw Exception");
        return static_cast<int>(round(volume * 100.0f));
    }

    void SystemVolume::Mute()
    {
        hr = pAudioEndpointVolume->SetMute(TRUE, nullptr);
        if (FAILED(hr))
            throw std::exception("Mute() throw Exception");
    }

    void SystemVolume::UnMute()
    {
        hr = pAudioEndpointVolume->SetMute(FALSE, nullptr);
        if (FAILED(hr))
            throw std::exception("UnMute() throw Exception");
    }
} // namespace SystemVolume