using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Signer;
using Nethereum.Hex.HexTypes;
using System;
using System.Threading.Tasks;

public class TransferEther : MonoBehaviour
{
    public InputField ReceiveAddress;
    public InputField Amount;
    public Text BalanceText;

    private Account myaccount;
    private decimal myBalance;
    private static string URL = "https://data-seed-prebsc-1-s1.binance.org:8545/";                          // RPC
    private static string PRIVATEKEY = "12cc96cf2ff9b640b13c29b543997a840d00a78dd2e281e1da6bd5356dbc7cb7";  // MetamaskTestnet

    private async void Start()
    {
        myaccount = new Account(PRIVATEKEY, 97);                                                            // Chainid = 97 or 0x61
        myBalance = await GetAccountTask(myaccount);
        BalanceText.text = string.Format("{0:0.0000} TBNB", myBalance);
    }

    public async void TransferETH()
    {

        var web3 = new Web3(myaccount, URL);
        web3.TransactionManager.UseLegacyAsDefault = true;
        var toAddress = ReceiveAddress.text.ToString();

        var transaction = await web3.Eth.GetEtherTransferService().TransferEtherAsync(toAddress, decimal.Parse(Amount.text));
    }

    private async Task<decimal> GetAccountTask(Account account)
    {
        Web3 web3 = new Web3(account, URL);
        var balanace = await web3.Eth.GetBalance.SendRequestAsync(account.Address);

        var etheramount = Web3.Convert.FromWei(balanace);
        Debug.Log($"<color=orange> {account.Address}  </color> possesses <color=orange> {etheramount} TBNB</color>");

        return etheramount;
    }
}
