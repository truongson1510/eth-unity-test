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

public class NethereumSample : MonoBehaviour
{

    public InputField inputPrivateKey;
    public Text addressText;
    public Text balanceText;

    private HexBigInteger ethBalance;
    private Web3 web3;
    private Account account;
    private string from;
    private Contract contract;
    private string url;
    private HexBigInteger balance;

    public void onConnectAccount()
    {
        AccountSetup(inputPrivateKey.text.ToString());
        addressText.text = "" + from;
    }

    public void AccountSetup(string privatekey)
    {
        url = "http://localhost:8545";
        account = new Account(privatekey);
        from = account.Address;
        web3 = new Web3(account, url);
        GetBalance();
    }

    private async void GetBalance()
    {
        await GetAccountBalance();
    }

    private async Task GetAccountBalance()
    {
        Debug.Log("Getting balance . . .");
        balance = await web3.Eth.GetBalance.SendRequestAsync(from);
        Debug.Log($"Balance in Wei: {balance.Value}");

        decimal etheramount = Web3.Convert.FromWei(balance.Value);
        balanceText.text = string.Format("{0:0.00} ETH", etheramount);
    }
}