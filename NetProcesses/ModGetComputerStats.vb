Module ModGetComputerStats

    Public myComputerName As String
    Public myIpAddress As String
    Public subnetMask As String
    Public myGateway As String
    Public dns1 As String
    Public dns2 As String

    Public Sub GetComputerStats()
        ' Courtesy of: https://stackoverflow.com/questions/40814462/get-ip-address-subnet-default-gateway-dns1-and-dns2-with-vb-net

        'Computer Name
        myComputerName = System.Net.Dns.GetHostName()
        For Each ip In System.Net.Dns.GetHostEntry(myComputerName).AddressList
            If ip.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                'IPv4 Adress
                myIpAddress = ip.ToString()

                For Each adapter As Net.NetworkInformation.NetworkInterface In Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                    For Each unicastIPAddressInformation As Net.NetworkInformation.UnicastIPAddressInformation In adapter.GetIPProperties().UnicastAddresses
                        If unicastIPAddressInformation.Address.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                            If ip.Equals(unicastIPAddressInformation.Address) Then
                                'Subnet Mask
                                subnetMask = unicastIPAddressInformation.IPv4Mask.ToString

                                Dim adapterProperties As Net.NetworkInformation.IPInterfaceProperties = adapter.GetIPProperties()
                                For Each gateway As Net.NetworkInformation.GatewayIPAddressInformation In adapterProperties.GatewayAddresses
                                    'Default Gateway
                                    myGateway = gateway.Address.ToString()
                                Next

                                'DNS1
                                If adapterProperties.DnsAddresses.Count > 0 Then
                                    dns1 = adapterProperties.DnsAddresses(0).ToString()
                                End If

                                'DNS2
                                If adapterProperties.DnsAddresses.Count > 1 Then
                                    dns2 = adapterProperties.DnsAddresses(1).ToString()
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        Next
    End Sub

End Module
