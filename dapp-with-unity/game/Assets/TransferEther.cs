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
    private static string URL = "http://localhost:8545";
    private static string PRIVATEKEY = "0xf9d11306d06e55a359f5a7ec2fe9536c02116b3b7c23041f0c4abb4978ed501d";

    private async void Start()
    {
        myaccount = new Account(PRIVATEKEY);
        myBalance = await GetAccountTask(myaccount);
        BalanceText.text = string.Format("{0:0.00} ETH", myBalance);
    }

    private async Task<decimal> GetAccountTask(Account account)
    {
        Web3 web3 = new Web3(account, URL);
        var balanace = await web3.Eth.GetBalance.SendRequestAsync(account.Address);

        var etheramount = Web3.Convert.FromWei(balanace);
        Debug.Log($"<color=orange> {account.Address}  </color> Possesses <color=orange> {etheramount} </color> ETH");

        return etheramount;
    }
}
