using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Configs;
using static UnityEngine.Rendering.STP;

namespace Assets.Scripts.Core
{
    public class FuelSystem
    {
        private float currentFuel;
        private float fuelConsumptionRate;
        private Action<float> onFuelChanged;

        public float MaxFuelDistance => currentFuel / fuelConsumptionRate;

        public float Currentfuel => currentFuel;
        public bool HasFuel => currentFuel > 0;

        public void Initialize(DrawLineSetupConfig config)
        {
            fuelConsumptionRate = config.fuelConsumptionRate;
        }

        public void SetMaxFuel(float maxFuel)
        {
            currentFuel = maxFuel;
        }

        public float GetRequiredFuelForDistance(float distance)
        {
            return distance * fuelConsumptionRate;
        }

        public bool HasEnoughFuelForDistance(float distance)
        {
            return currentFuel >= GetRequiredFuelForDistance(distance);
        }

        public bool ConsumeFuelForDistance(float distance)
        {
            float required = GetRequiredFuelForDistance(distance);
            if (currentFuel >= required)
            {
                currentFuel -= required;
                onFuelChanged?.Invoke(currentFuel);
                return true;
            }
            return false;
        }

        public void AddFuel(float amount)
        {
            currentFuel += amount;
            onFuelChanged?.Invoke(currentFuel);
        }

        public float GetPossibleDistance()
        {
            return currentFuel / fuelConsumptionRate;
        }

        public void RegisterActionFuelChanged(Action<float> onFuelChanged)
        {
            this.onFuelChanged += onFuelChanged;
        }
        public void UnregisterActionFuelChanged(Action<float> onFuelChanged)
        {
            this.onFuelChanged -= onFuelChanged;
        }
    }
}