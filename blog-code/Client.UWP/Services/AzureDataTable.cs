﻿using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Client.UWP.Services
{
    public class AzureDataTable<T> where T: EntityData
    {
        private IMobileServiceTable<T> dataTable;
        private ObservableCollection<T> dataView;

        public AzureDataTable(IMobileServiceTable<T> table)
        {
            this.dataTable = table;
        }

        public async Task SaveAsync(T item)
        {
            try
            {
                if (item.Id == null)
                {
                    await dataTable.InsertAsync(item);
                    dataView.Add(item);
                }
                else
                {
                    await dataTable.UpdateAsync(item);
                    // Remove the old version and Add the new version
                    dataView.Remove(item);
                    dataView.Add(item);
                }
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                throw new CloudTableOperationFailed(exception.Message, exception);
            }
        }

        public async Task DeleteAsync(T item)
        {
            try
            {
                await dataTable.DeleteAsync(item);
                dataView.Remove(item);
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                throw new CloudTableOperationFailed(exception.Message, exception);
            }

        }

        public async Task<ObservableCollection<T>> RefreshAsync()
        {
            try
            {
                IEnumerable<T> items = await dataTable.OrderBy(item => item.UpdatedAt).ToEnumerableAsync();
                dataView = new ObservableCollection<T>();
                return dataView;
            }
            catch (MobileServiceInvalidOperationException exception)
            {
                throw new CloudTableOperationFailed(exception.Message, exception);
            }
        }
    }
}