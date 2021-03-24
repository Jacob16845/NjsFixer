﻿using System;
using System.Collections.Generic;
using System.Linq;
using HMUI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;


namespace NjsFixer.UI
{
    internal class PreferencesListViewController : BSMLResourceViewController
    {
        public override string ResourceName => "NjsFixer.UI.BSML.preferencesList.bsml";

        [UIValue("minJump")]
        private int minJump => Config.UserConfig.minJumpDistance;
        [UIValue("maxJump")]
        private int maxJump => Config.UserConfig.maxJumpDistance;

        [UIComponent("prefList")]
        public CustomListTableData prefList;
        private NjsPref _selectedPref = null;
        [UIValue("prefIsSelected")]
        public bool prefIsSelected
        {
            get => _selectedPref != null;
            set
            {
                NotifyPropertyChanged();
            }
        }
        private float _newNjs = 16f;
        [UIValue("njsValue")]
        public float njsValue
        {
            get => _newNjs;
            set
            {
                _newNjs = value;
            }
        }
        [UIAction("setNjs")]
        void SetNjs(float value)
        {
            njsValue = value;
        }
        private float _newJumpDis = 23f;
        [UIValue("jumpDisValue")]
        public float jumpDisValue
        {
            get => _newJumpDis;
            set
            {
                _newJumpDis = value;
            }
        }
        [UIAction("setJumpDis")]
        void SetJumpDis(float value)
        {
            jumpDisValue = value;
        }

        [UIAction("#post-parse")]
        private void Init()
        {
            ReloadListFromConfig();
        }
        [UIAction("prefSelect")]
        private void SelectedPref(TableView tableView, int row)
        {
            Logger.log.Debug("Selected row " + row);
            _selectedPref = Config.UserConfig.preferredValues[row];
            prefIsSelected = prefIsSelected;
        }
        [UIAction("addPressed")]
        private void AddNewValue()
        {
            if(Config.UserConfig.preferredValues.Any(x => x.njs == _newNjs))
            {
                Config.UserConfig.preferredValues.RemoveAll(x => x.njs == _newNjs);
            }
            Config.UserConfig.preferredValues.Add(new NjsPref(_newNjs, _newJumpDis));
            Config.Write();
            ReloadListFromConfig();
        }
        [UIAction("removePressed")]
        private void RemoveSelectedPref()
        {
            if (_selectedPref == null) return;
            Config.UserConfig.preferredValues.RemoveAll(x => x == _selectedPref);
            Config.Write();
            ReloadListFromConfig();
        }
        private void ReloadListFromConfig()
        {
            prefList.data.Clear();
            foreach(var pref in Config.UserConfig.preferredValues)
            {
                prefList.data.Add(new CustomListTableData.CustomCellInfo($"{pref.njs} NJS | {pref.jumpDistance} Jump Distance"));
            }
            prefList.tableView.ReloadData();
            prefList.tableView.ClearSelection();
            _selectedPref = null;
            prefIsSelected = prefIsSelected;
        }
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            if (!firstActivation)
            {
                Init();
            }
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }
    }
}
