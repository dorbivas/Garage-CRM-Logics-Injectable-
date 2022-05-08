﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        public enum eGarageOptions
        {
            CloseTheBasta,
            InsertVehicle,
            DisplayListOfLicensedVehicle,
            FillTirePressure,
            fillGas,
            ChangeVehicleState,
            ChargeElectricVehicle,
            ExtendedInformationOfSelectedVehicle,
        }

        //todo
        private readonly Dictionary<string, Vehicle> r_GarageVehicles;
        private readonly VehicleFactory r_Factory;
        //private readonly eGarageOptions m_Options;
        const int k_NumberOfAvailableMethodsInGarage = 0;
        const string k_ParsingToIntErrorFlag = "0";
        const float k_MaxPercentage = 100;
        const float k_MinPercentage = 0;
        //const int k_MaxNumberOfStatuses = 0; 
        //const int k_MinNumberOfStatuses = 0;

        public Garage()
        {
            r_GarageVehicles = new Dictionary<string, Vehicle>();
            r_Factory = new VehicleFactory();
        }

        public Dictionary<string, Vehicle>.KeyCollection PlatesList
        {
            get => r_GarageVehicles.Keys;
        }

        public VehicleFactory Factory
        {
            get => r_Factory;
        }

        ////public eGarageOptions Options 
        //{
        //    get => m_Options;
        //}

        public Vehicle GetVehicle(string i_LicenseNumber)
        {

            if (!r_GarageVehicles.TryGetValue(i_LicenseNumber, out Vehicle copiedVehicle))
            {
                throw new KeyNotFoundException(i_LicenseNumber);
            }
            return r_GarageVehicles[i_LicenseNumber];
        }

        public void AddVehicle(Vehicle i_Vehicle, string i_LicenseNumber)
        {
            r_GarageVehicles.Add(i_LicenseNumber, i_Vehicle);
        }

        public Dictionary<string, Vehicle>.KeyCollection GetPlatesList()
        {
            return r_GarageVehicles.Keys;
        }
        
        public void FillTyresMaxAirPressure(string i_LicenseNumber)
        {
            bool isValueFatched = r_GarageVehicles.TryGetValue(i_LicenseNumber, out Vehicle currentVehicle);

            if (isValueFatched)
            {
                foreach (VehicleParts.Wheel wheel in r_GarageVehicles[i_LicenseNumber].Wheels)
                {
                    wheel.FillMaxPressure();
                }
            }
            else
            {
                throw new KeyNotFoundException($"Error:Could Not find {i_LicenseNumber} Vehicle");
            }
        }

        public void AddFuel(string i_LicenseNumber, VehicleParts.CombustionEngine.eFuelType i_FuelType, float i_AmountToFill)
        {
            r_GarageVehicles.TryGetValue(i_LicenseNumber, out Vehicle currentVehicle);
            ((VehicleParts.CombustionEngine)currentVehicle.Engine).AddFuel(i_AmountToFill, i_FuelType);
        }

        public void CheckIfEngineIsCombustion(string i_LicenseNumber)
        {
            r_GarageVehicles.TryGetValue(i_LicenseNumber, out Vehicle currentVehicle);
            if (!(currentVehicle.Engine is VehicleParts.CombustionEngine))
            {
                throw new FormatException("cant fill gas on electric engine!");
            }
        }

        public void Charge(string i_LicenseNumber, float i_EnergyToAdd)
        {
            r_GarageVehicles.TryGetValue(i_LicenseNumber, out Vehicle currentVehicle);
            ((VehicleParts.ElectricEngine)currentVehicle.Engine).SuperCharge(i_EnergyToAdd);
        }

        public void CheckIfEngineIsElectric(string i_LicenseNumber)
        {
            r_GarageVehicles.TryGetValue(i_LicenseNumber, out Vehicle currentVehicle);
            if (!(currentVehicle.Engine is VehicleParts.ElectricEngine))
            {
                throw new FormatException("cant charge fuel engine!");
            }
        }

        public void CheckPressureInputAndAdd(string i_addedAirPressure, VehicleParts.Wheel i_Wheel)
        {
            bool isNumeric;
            int parsedInteger;

            isNumeric = int.TryParse(i_addedAirPressure, out parsedInteger);
            if (isNumeric)
            {
                i_Wheel.AddAir(parsedInteger);
            }
            else
            {
                throw new FormatException(Garage.k_NotIntError);
            }
        }

        public Vehicle.eVehicleStatus GetVehicleStatus(string i_PlatesList)
        {
            r_GarageVehicles.TryGetValue(i_PlatesList, out Vehicle currentVehicle); // todo use exception in catch ?
            return currentVehicle.Status;
        }

        public int GetNumberOfWheels(string i_LicenseNumber)
        {
            return r_GarageVehicles[i_LicenseNumber].NumberOfWheels;
        }

        public const string k_NotIntError = "Error: non Integer number entered!";

        public bool CheckIfVehicleExists(string i_Input)
        {
            int parsedInt;
  
            if (!int.TryParse(i_Input, out parsedInt))
            {
                throw new FormatException(k_NotIntError);
            }

            return r_GarageVehicles.ContainsKey(i_Input);
        }

    }
}
