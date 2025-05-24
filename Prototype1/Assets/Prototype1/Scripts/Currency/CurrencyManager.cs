using Assets.Prototype1.Scripts.Buildings;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Prototype1.Scripts
{
    public class CurrencyManager : MonoBehaviour
    {
        public static CurrencyManager Instance { get; private set; }

        [SerializeField] private int _startingCurrency = 100;
        [SerializeField]
        private int _currentCurrency = 0;
        [SerializeField] private List<ResourceBuilding> _resourceBuilding = new List<ResourceBuilding> ();
        [SerializeField] private TMP_Text _currencyText;

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _currentCurrency = _startingCurrency;
        }

        public bool SpendCurrency(int cost)
        {
            if(_currentCurrency < cost)
            {
                Debug.LogError("Not Enough Currency");
                return false;
            }

            _currentCurrency -= cost;
            Debug.Log($"Currency Spent: {cost}, Remaining :{_currentCurrency}");
            return true;
        }

        public int GetCurrency()=> _currentCurrency;

        private void AddCurrency(int amount)
        {
            _currentCurrency += amount;
            Debug.Log($"Currency added: {amount}, Total: {_currentCurrency}");
        }
        public void CollectDailyIncome()
        {
            int totalIncome = 0;
            var activeBuildings = _resourceBuilding.FindAll(b => b.State == BuildingState.Completed);
            foreach(var building in activeBuildings)
            {
                totalIncome += building.GenerateResources();
            }

            AddCurrency(totalIncome);
            Debug.Log($"Daily income Collected: {totalIncome}");
        }

        // Update is called once per frame
        void Update()
        {
            _currencyText.text = _currentCurrency.ToString();
        }
    }
}