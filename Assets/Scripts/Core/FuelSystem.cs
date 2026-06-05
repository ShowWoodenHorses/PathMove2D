using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Core
{
    public class FuelSystem
    {
        private float maxFuel;
        private float currentFuel;
        private float fuelConsumptionRate;
        private Image fuelBar;
        private Action<float> onFuelChanged;

        public float MaxFuelDistance => currentFuel / fuelConsumptionRate;

        public float Currentfuel => currentFuel;
        public bool HasFuel => currentFuel > 0;

        public void Initialize(DrawLineSetupConfig config)
        {
            maxFuel = config.maxFuel;
            fuelConsumptionRate = config.fuelConsumptionRate;
            fuelBar = config.fuelBar;
            currentFuel = config.maxFuel;

            UpdateFuelUI();
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
                UpdateFuelUI();
                onFuelChanged?.Invoke(currentFuel);
                return true;
            }
            return false;
        }

        public void AddFuel(float amount)
        {
            currentFuel = Mathf.Min(currentFuel + amount, maxFuel);
            UpdateFuelUI();
            onFuelChanged?.Invoke(currentFuel);
        }

        public float GetPossibleDistance()
        {
            return currentFuel / fuelConsumptionRate;
        }

        public void RegisterActionFuelChanged(Action<float> onFuelChanged)
        {
            this.onFuelChanged = onFuelChanged;
        }

        private void UpdateFuelUI()
        {
            if (fuelBar != null)
                fuelBar.fillAmount = currentFuel / maxFuel;
        }
    }
}